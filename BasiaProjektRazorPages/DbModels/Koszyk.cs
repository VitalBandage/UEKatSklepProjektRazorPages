﻿namespace BasiaProjektRazorPages.DbModels
{
    public class Koszyk
    {
        public int ID_Koszyku { get; set; }
        public int ID_Zamowienia { get; set; }

        public int ID_Produktu { get; set; }

        public int Ilosc_produktow { get; set; }

        public int ID_Koszyka { get; set; }
    }
}
