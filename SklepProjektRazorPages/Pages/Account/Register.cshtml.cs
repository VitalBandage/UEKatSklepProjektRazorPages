﻿using SklepProjektRazorPages.Helpers;
using SklepProjektRazorPages.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Text.RegularExpressions;
using Dapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace SklepProjektRazorPages.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public string userName { get; set; }
        [BindProperty]
        public string email { get; set; }
        [BindProperty]
        public string repeatedPassword { get; set; }
        [BindProperty]
        public string password { get; set; }

        public string alertClass { get; set; } = "alert-warning";
        public string alertValue { get; set; } = "";
        public bool accountCreated = false;

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            bool ok = true;
            var mailRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
            // username
            if (string.IsNullOrWhiteSpace(userName) || userName.Length < 6)
            {
                ok = false;
                alertClass = "alert-danger";
                alertValue = "Nazwa użytkownika musi mieć conajmniej 6 znaków.";
            }
            int nameTaken = 0;
            using (IDbConnection conn = DbHelper.GetDbConnection())
                nameTaken = conn.ExecuteScalar<int>($"SELECT COUNT(*) FROM Konto WHERE LoginUzytkownika = '{userName}'");
            if (nameTaken > 0)
            {
                ok = false;
                alertClass = "alert-danger";
                if (string.IsNullOrEmpty(alertValue) == false)
                    alertValue += "\n";
                alertValue += "Ta nazwa użytkownika jest już zajęta.";
            }
            // password
            if(string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                ok = false;
                alertClass = "alert-danger";
                if (string.IsNullOrEmpty(alertValue) == false)
                    alertValue += "\n";
                alertValue += "Hasło musi mieć conajmniej 6 znaków.";
            }
            if(string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(repeatedPassword) || password.Trim() != repeatedPassword.Trim())
            {
                ok = false;
                alertClass = "alert-danger";
                if (string.IsNullOrEmpty(alertValue) == false)
                    alertValue += "\n";
                alertValue += "Żeś chujowo te hasło powtórzył(a).";
            }
            // e-mail
            if(string.IsNullOrWhiteSpace(email) || mailRegex.IsMatch(email) == false) 
            {
                ok = false;
                if (string.IsNullOrEmpty(alertValue) == false)
                    alertValue += "\n";
                alertValue += "E-mail jest nieprawidłowy. 😡";
            }
            if (ok)
            {
                using (IDbConnection conn = DbHelper.GetDbConnection())
                {
                    DateTime creation = DateTime.Now;
                    string passwordHash = AccountHelper.hashPassword(password, creation.ToString("s"));
                    int clientId = this.createEmptyClientDbEntry(); // <- not finished
                    var result = conn.Query($"INSERT INTO Konto (ID_Klienta, LoginUzytkownika, Email, HashHasla, DataUtworzenia, JestAdminem, Aktywny) VALUES({clientId}, '{userName}', '{email}', '{passwordHash}', '{creation.ToString("s")}', 1, 1)");
                    alertClass = "alert-success";
                    alertValue = "Pomyślnie utworzono!\nNa podany email został posłany link aktywacyjny.";
                    accountCreated = true;
                }
            }
            return Page();
        }

        public int createEmptyClientDbEntry()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                string nullPropsSql = "(";
                int propsCount = new Klient().GetType().GetProperties().Length - 1;
                for (int i = 0; i < propsCount; i++)
                {
                    
                    if (i + 1 == propsCount)
                        nullPropsSql += "NULL";
                    else
                        nullPropsSql += "NULL, ";
                }
                nullPropsSql += ")";
                int clientId = conn.ExecuteScalar<int>($"INSERT INTO Klient VALUES{nullPropsSql}; SELECT SCOPE_IDENTITY();");
                return clientId;
            }
        }
    }
}
