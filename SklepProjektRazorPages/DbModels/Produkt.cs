﻿using System.Text.RegularExpressions;

namespace SklepProjektRazorPages.DbModels
{
    public class Produkt : BaseDbModel
    {
        public int? ID_Produktu { get; set; }
        public string Nazwa { get; set; }
        public int? Cena_jednostkowa { get; set; }
        public int? ID_Kategorii { get; set; }
        public string sciezkaZdjecia { get; set; }
        public bool usuniety { get; set; }

        public static Tuple<bool, string> VerifyValues(string name, int? price)
        {
            Regex nonDigit = new Regex(@"/D");
            bool ok = true;
            string msg = "";
            // name
            if(name != null && (string.IsNullOrWhiteSpace(name)))
            {
                ok = false;
                msg = "Nazwa produktu jest pusta";
            }

            // price
            float result;
            if (price != null)
            {
                if (price < 0)
                {
                    ok = false;
                    if (!string.IsNullOrEmpty(msg))
                        msg += "\n";
                    msg += "Cena nie może być ujemna";
                }
                else if(price == default)
                {
                    ok = false;
                    if (!string.IsNullOrEmpty(msg))
                        msg += "\n";
                    msg += "Cena nie może być pusta";
                }
            }

            return new Tuple<bool, string>(ok, msg);
        }
        public Tuple<bool, string> VerifyInstanceValues(bool ignoreNullProps = false)
        {
            if (ignoreNullProps)
                return Produkt.VerifyValues(this.Nazwa, this.Cena_jednostkowa);
            else
            {
                Produkt temp = (Produkt)this.MemberwiseClone();
                temp.changeNullValueTypePropertiesToDefaultValues();
                return Produkt.VerifyValues(temp.Nazwa, temp.Cena_jednostkowa);
            }
        }
    }
}
