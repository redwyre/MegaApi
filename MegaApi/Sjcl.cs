using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MegaApi
{
    public class Sjcl
    {
        public class Cipher
        {
            public class Aes
            {
                public static uint[][][] _tables = new uint[][][] { new uint[][] { new uint[256], new uint[256], new uint[256], new uint[256], new uint[256] }, new uint[][] { new uint[256], new uint[256], new uint[256], new uint[256], new uint[256] } };

                public uint[][] _key;

                static Aes()
                {
                    var encTable = _tables[0];
                    var decTable = _tables[1];
                    var sbox = encTable[4];
                    var sboxInv = decTable[4];
                    uint x, xInv;
                    uint[] d = new uint[256];
                    uint[] th = new uint[256];
                    uint x2, x4, x8, s;
                    uint tEnc, tDec;

                    // Compute double and third tables
                    for (uint i = 0; i < 256; i++)
                    {
                        d[i] = i << 1 ^ (i >> 7) * 283;
                        th[d[i] ^ i] = i;
                    }

                    for (x = xInv = 0; sbox[x] == 0; x ^= (x2 != 0 ? x2 : 1), xInv = (th[xInv] != 0 ? th[xInv] : 1))
                    {
                        // Compute sbox
                        s = xInv ^ xInv << 1 ^ xInv << 2 ^ xInv << 3 ^ xInv << 4;
                        s = s >> 8 ^ s & 255 ^ 99;
                        sbox[x] = s;
                        sboxInv[s] = x;

                        // Compute MixColumns
                        x8 = d[x4 = d[x2 = d[x]]];
                        tDec = x8 * 0x1010101 ^ x4 * 0x10001 ^ x2 * 0x101 ^ x * 0x1010100;
                        tEnc = d[s] * 0x101 ^ s * 0x1010100;

                        for (int i = 0; i < 4; i++)
                        {
                            encTable[i][x] = tEnc = tEnc << 24 ^ tEnc >> 8;
                            decTable[i][s] = tDec = tDec << 24 ^ tDec >> 8;
                        }
                    }

                    //// Compactify.  Considerable speedup on Firefox.
                    //for (int i = 0; i < 5; i++) {
                    //    encTable[i] = encTable[i].slice(0);
                    //    decTable[i] = decTable[i].slice(0);
                    //}
                }

                public Aes(uint[] key)
                {
                    int i, j;
                    uint tmp;
                    uint[] sbox = _tables[0][4];
                    var decTable = _tables[1];
                    int keyLen = key.Length;
                    uint rcon = 1;
                    uint[] encKey = new uint[4 * keyLen + 28];
                    uint[] decKey = new uint[4 * keyLen + 28];

                    if (keyLen != 4 && keyLen != 6 && keyLen != 8)
                    {
                        throw new Exception("invalid aes key size");
                    }
                    Array.Copy(key, 0, encKey, 0, keyLen);
                    this._key = new uint[][] { encKey, decKey };

                    // schedule encryption keys
                    for (i = keyLen; i < 4 * keyLen + 28; i++)
                    {
                        tmp = encKey[i - 1];

                        // apply sbox
                        if (i % keyLen == 0 || (keyLen == 8 && i % keyLen == 4))
                        {
                            tmp = sbox[tmp >> 24] << 24 ^ sbox[tmp >> 16 & 255] << 16 ^ sbox[tmp >> 8 & 255] << 8 ^ sbox[tmp & 255];

                            // shift rows and add rcon
                            if ((i % keyLen) == 0)
                            {
                                tmp = (tmp << 8) ^ (tmp >> 24) ^ (rcon << 24);
                                rcon = rcon << 1 ^ (rcon >> 7) * 283;
                            }
                        }

                        encKey[i] = encKey[i - keyLen] ^ tmp;
                    }

                    // schedule decryption keys
                    for (j = 0; i != 0; j++, i--)
                    {
                        tmp = encKey[(j & 3) != 0 ? i : i - 4];
                        if (i <= 4 || j < 4)
                        {
                            decKey[j] = tmp;
                        }
                        else
                        {
                            decKey[j] = decTable[0][sbox[tmp >> 24]] ^
                                        decTable[1][sbox[tmp >> 16 & 255]] ^
                                        decTable[2][sbox[tmp >> 8 & 255]] ^
                                        decTable[3][sbox[tmp & 255]];
                        }
                    }
                }

                public uint[] Encrypt(uint[] data) { return this.Crypt(data, 0); }

                public uint[] Decrypt(uint[] data) { return this.Crypt(data, 1); }

                public uint[] Crypt(uint[] input, int dir)
                {
                    if (input.Length != 4)
                    {
                        throw new Exception("invalid aes block size");
                    }

                    var key = this._key[dir];
                    // state variables a,b,c,d are loaded with pre-whitened data
                    uint a = input[0] ^ key[0];
                    uint b = input[(dir != 0) ? 3 : 1] ^ key[1];
                    uint c = input[2] ^ key[2];
                    uint d = input[(dir != 0) ? 1 : 3] ^ key[3];
                    uint a2, b2, c2;

                    int nInnerRounds = key.Length / 4 - 2;
                    uint kIndex = 4;
                    uint[] outBuffer = new uint[] { 0, 0, 0, 0 };
                    var table = _tables[dir];

                    // load up the tables
                    var t0 = table[0];
                    var t1 = table[1];
                    var t2 = table[2];
                    var t3 = table[3];
                    uint[] sbox = table[4];

                    // Inner rounds.  Cribbed from OpenSSL.
                    for (int i = 0; i < nInnerRounds; i++)
                    {
                        a2 = t0[a >> 24] ^ t1[b >> 16 & 255] ^ t2[c >> 8 & 255] ^ t3[d & 255] ^ key[kIndex];
                        b2 = t0[b >> 24] ^ t1[c >> 16 & 255] ^ t2[d >> 8 & 255] ^ t3[a & 255] ^ key[kIndex + 1];
                        c2 = t0[c >> 24] ^ t1[d >> 16 & 255] ^ t2[a >> 8 & 255] ^ t3[b & 255] ^ key[kIndex + 2];
                        d = t0[d >> 24] ^ t1[a >> 16 & 255] ^ t2[b >> 8 & 255] ^ t3[c & 255] ^ key[kIndex + 3];
                        kIndex += 4;
                        a = a2; b = b2; c = c2;
                    }

                    // Last round.
                    for (int i = 0; i < 4; i++)
                    {
                        outBuffer[(dir != 0) ? 3 & -i : i] =
                          sbox[a >> 24] << 24 ^
                          sbox[b >> 16 & 255] << 16 ^
                          sbox[c >> 8 & 255] << 8 ^
                          sbox[d & 255] ^
                          key[kIndex++];
                        a2 = a; a = b; b = c; c = d; d = a2;
                    }

                    return outBuffer;
                }
            }
        }
    }
}
