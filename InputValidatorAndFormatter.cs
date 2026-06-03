using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace ERGOLAB_KPL
{
    public class InputValidatorAndFormatter
    {
        // Regex untuk validasi nomor telepon Indonesia
        private static readonly Regex PhoneRegex = new Regex(@"^(08|\+628)[0-9]{8,11}$", RegexOptions.Compiled);

        // Regex untuk validasi NIK (16 digit)
        private static readonly Regex NikRegex = new Regex(@"^[0-9]{16}$", RegexOptions.Compiled);

        // Defensive Programming / Design by Contract (Pre-condition & Post-condition)
        public bool ValidateNoTelp(string noTelp)
        {
            // Pre-condition: Antisipasi input kosong/null
            if (string.IsNullOrWhiteSpace(noTelp)) return false;
            return PhoneRegex.IsMatch(noTelp.Trim());
        }

        public bool ValidateNIK(string nik)
        {
            // Pre-condition
            if (string.IsNullOrWhiteSpace(nik)) return false;
            return NikRegex.IsMatch(nik.Trim());
        }

        public string SamarkanNamaPelapor(string namaOriginal)
        {
            // Pre-condition: Sesuai Contract, nama tidak boleh kosong saat disamarkan
            if (string.IsNullOrWhiteSpace(namaOriginal))
            {
                throw new ArgumentException("Nama pelapor tidak boleh kosong.", nameof(namaOriginal));
            }

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(namaOriginal.Trim());
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                string namaSamaran = "WARGA-" + sb.ToString().Substring(0, 8);

                // Post-condition: Memastikan output yang keluar valid
                if (string.IsNullOrEmpty(namaSamaran))
                {
                    throw new InvalidOperationException("Gagal enkripsi nama.");
                }

                return namaSamaran;
            }
        }
    }
}