using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps manipulate Criptography operations
    /// </summary>
    /// <remarks>Can't be debugged</remarks>
    [Browsable(false), DebuggerStepThrough]
    public static class CryptographHelper
    {
        #region "Rijndael Encrypt and Decrypt"

        /// <summary>
        /// Encrypts a text using a given password
        /// </summary>
        public static string RijndaelEncrypt(string text, string password)
        {
            string encryptedText;

            try
            {
                var pdb = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(password));
                var alg = Rijndael.Create();
                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16);

                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
                var clearBytes = Encoding.UTF8.GetBytes(text);
                cs.Write(clearBytes, 0, clearBytes.Length);
                cs.Close();

                encryptedText = Convert.ToBase64String(ms.ToArray());

                ms.Close();
            }
            catch (CryptographicException ex)
            {
                throw new CryptographicException("Error to encrypt data: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Error to encrypt data: " + ex.Message);
            }

            return encryptedText;
        }

        /// <summary>
        /// Decrypts a text using a given password
        /// </summary>
        public static string RijndaelDecrypt(string text, string password)
        {
            string decryptedText;

            try
            {
                var pdb = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(password));
                var alg = Rijndael.Create();
                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16);

                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
                var cipherBytes = Convert.FromBase64String(text);
                cs.Write(cipherBytes, 0, cipherBytes.Length);
                cs.Close();

                decryptedText = Encoding.UTF8.GetString(ms.ToArray());
                
                ms.Close();
            }
            catch (CryptographicException ex)
            {
                throw new CryptographicException("Error to decrypt data: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Error to decrypt data: " + ex.Message);
            }

            return decryptedText;
        }

        #endregion

        #region "RSA Encrypt and Decrypt"

        /// <summary>
        /// Encrypts a text using a given Xml Public Key (RSA Format)
        /// </summary>
        public static string RsaEncrypt(string text, string xmlPublicKey)
        {
            var encryptedData = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(xmlPublicKey))
                    throw new CryptographicException("Public key not found.");

                using (var rsaProvider = new RSACryptoServiceProvider())
                {
                    RSACryptoServiceProvider.UseMachineKeyStore = false;
                    rsaProvider.FromXmlString(xmlPublicKey);

                    var byteClearText = Encoding.UTF8.GetBytes(text);
                    var encryptedBytes = rsaProvider.Encrypt(byteClearText, false);

                    if (encryptedBytes.Length > 0)
                        encryptedData = Convert.ToBase64String(encryptedBytes);

                    rsaProvider.Clear();
                }
            }
            catch (CryptographicException ex)
            {
                throw new CryptographicException("Error when encrypt data: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Error when encrypt data: " + ex.Message);
            }

            return encryptedData;
        }

        /// <summary>
        /// Decrypts a text using a given Xml Private Key (RSA Format)
        /// </summary>
        public static string RsaDecrypt(string text, string xmlPrivateKey)
        {
            var decryptedData = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(xmlPrivateKey))
                    throw new CryptographicException("Private key not found.");

                using (var rsaProvider = new RSACryptoServiceProvider())
                {
                    RSACryptoServiceProvider.UseMachineKeyStore = false;
                    rsaProvider.FromXmlString(xmlPrivateKey);

                    var byteClearText = Convert.FromBase64String(text);
                    var decryptedBytes = rsaProvider.Decrypt(byteClearText, false);

                    if (decryptedBytes.Length > 0)
                        decryptedData = Encoding.UTF8.GetString(decryptedBytes);

                    rsaProvider.Clear();
                }
            }
            catch (CryptographicException ex)
            {
                throw new CryptographicException("Error when decrypt data: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Error when decrypt data: " + ex.Message);
            }

            return decryptedData;
        }

        /// <summary>
        /// Encrypts a text using a given public key X509 certificate
        /// </summary>
        public static string RsaEncrypt(string text, X509Certificate2 certificate)
        {
            var encryptedData = string.Empty;

            try
            {
                if (certificate == null || string.IsNullOrEmpty(certificate.GetPublicKeyString()))
                    throw new CryptographicException("Public key not found.");

                using (var rsaProvider = (RSACryptoServiceProvider)certificate.PublicKey.Key)
                {
                    byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
                    byte[] encryptedBytes = rsaProvider.Encrypt(inputByteArray, false);

                    if (encryptedBytes.Length > 0)
                        encryptedData = Convert.ToBase64String(encryptedBytes);
                }
            }
            catch (CryptographicException ex)
            {
                throw new CryptographicException("Error when encrypt data: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Error when encrypt data: " + ex.Message);
            }

            return encryptedData;
        }

        /// <summary>
        /// Decrypts a text using a given private key X509 certificate
        /// </summary>
        public static string RsaDecrypt(string text, X509Certificate2 privateKeyCertificate)
        {
            var decryptedData = string.Empty;

            try
            {
                if (privateKeyCertificate == null || !privateKeyCertificate.HasPrivateKey)
                    throw new CryptographicException("Private key not found.");

                using (var rsaProvider = (RSACryptoServiceProvider)privateKeyCertificate.PrivateKey)
                {
                    var inputByteArray = Convert.FromBase64String(text);
                    byte[] decryptedBytes = rsaProvider.Decrypt(inputByteArray, false);

                    if (decryptedBytes.Length > default(int))
                        decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                }
            }
            catch (CryptographicException ex)
            {
                throw new CryptographicException("Error when decrypt data: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Error when decrypt data: " + ex.Message);
            }

            return decryptedData;
        }

        #endregion

        #region "TripleDES Encrypt and Decrypt"

        /// <summary>
        /// Encrypts a text using a given password
        /// </summary>
        public static string TripleDESEncrypt(string text, string password)
        {
            string encryptedText;

            try
            {
                var pdb = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(password));
                var alg = TripleDES.Create();
                alg.Key = pdb.GetBytes(24);
                alg.IV = pdb.GetBytes(8);

                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
                var clearBytes = Encoding.UTF8.GetBytes(text);
                cs.Write(clearBytes, 0, clearBytes.Length);
                cs.Close();

                encryptedText = Convert.ToBase64String(ms.ToArray());

                ms.Close();
            }
            catch (CryptographicException ex)
            {
                throw new CryptographicException("Error to encrypt data: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Error to encrypt data: " + ex.Message);
            }

            return encryptedText;
        }

        /// <summary>
        /// Decrypts a text using a given password
        /// </summary>
        public static string TripleDESDecrypt(string text, string password)
        {
            string decryptedText;

            try
            {
                var pdb = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(password));
                var alg = TripleDES.Create();
                alg.Key = pdb.GetBytes(24);
                alg.IV = pdb.GetBytes(8);

                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
                var cipherBytes = Convert.FromBase64String(text);
                cs.Write(cipherBytes, 0, cipherBytes.Length);
                cs.Close();

                decryptedText = Encoding.UTF8.GetString(ms.ToArray());

                ms.Close();
            }
            catch (CryptographicException ex)
            {
                throw new CryptographicException("Error to encrypt data: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Error to encrypt data: " + ex.Message);
            }

            return decryptedText;
        }

        #endregion
    }
}