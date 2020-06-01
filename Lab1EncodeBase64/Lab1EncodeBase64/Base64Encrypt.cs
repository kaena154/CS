using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1EncodeBase64
{
    class Base64Encrypt
    {
        byte[] source;
        int lengthText, lengthEncrypt;
        int blockCount;
        int paddingCount;
        
        public Base64Encrypt(byte[] text)
        {
            source = text;
            lengthText = text.Length;
            
            if (lengthText % 3 == 0)
            {
                paddingCount = 0;
                blockCount = lengthText / 3;
            }
            else
            {
                paddingCount = 3 - (lengthText % 3);
                blockCount = (lengthText + paddingCount) / 3;
            }
            lengthEncrypt = lengthText + paddingCount;
        }

        public char[] GetEncoded()
        {
            byte b1, b2, b3;
            byte temp, temp1, temp2, temp3, temp4;
            byte[] buffer = new byte[blockCount * 4];
            byte[] source2;
            char[] result = new char[blockCount * 4];
            source2 = new byte[lengthEncrypt];

            for (int i = 0; i < lengthEncrypt; i++)
            {
                if (i < lengthText)
                {
                    source2[i] = source[i];
                }
                else
                {
                    source2[i] = 0;
                }
            }

            for (int i = 0; i < blockCount; i++)
            {
                b1 = source2[i * 3];
                b2 = source2[i * 3 + 1];
                b3 = source2[i * 3 + 2];

                temp1 = (byte)((b1 & 252) >> 2);

                temp = (byte)((b1 & 3) << 4);
                temp2 = (byte)((b2 & 240) >> 4);
                temp2 += temp; 

                temp = (byte)((b2 & 15) << 2);
                temp3 = (byte)((b3 & 192) >> 6);
                temp3 += temp;

                temp4 = (byte)(b3 & 63);

                buffer[i * 4] = temp1;
                buffer[i * 4 + 1] = temp2;
                buffer[i * 4 + 2] = temp3;
                buffer[i * 4 + 3] = temp4;
            }

            for (int i = 0; i < blockCount * 4; i++)
            {
                result[i] = SymbolsTable(buffer[i]);
            }

            switch (paddingCount)
            {
                case 0:
                    break;
                case 1:
                    result[blockCount * 4 - 1] = '=';
                    break;
                case 2:
                    result[blockCount * 4 - 1] = '=';
                    result[blockCount * 4 - 2] = '=';
                    break;
                default:
                    break;
            }
            return result;
        }

        private char SymbolsTable(byte num)
        {
            char[] symbolsTable = new char[64]
            {   'A','B','C','D','E','F','G','H','I','J','K','L','M',
                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                '0','1','2','3','4','5','6','7','8','9','+','/'
            };

            if ((num >= 0) && (num <= 63))
            {
                return symbolsTable[num];
            }
            else
            {
                return ' ';
            }
        }
    }
}
