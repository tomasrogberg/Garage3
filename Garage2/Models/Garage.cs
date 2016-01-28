using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
  
    class Garage
    {

       bool[] gar;
        //  public int MaxSpaceKm { get; set; } // ytan i kvadratmeter
        public Garage()
        {
            Max = 1000;
            gar = new bool[Max];

        }

        public Garage(int _max)
        {
            //maxSpaceKm = _maxSpaceKm;
            Max = _max;
            gar = new bool[Max];
        }

        public int Max { get; set; }

        //T[] TArr; // initieras i konstruktorn   ARRAY OF FORDON !!!

        public int Add()  // returnera platsen
        {
            for (int i = 1; i < Max - 1; i++)
            {
                if (gar[i] == false)
                {
                    gar[i] = true;
                    return i; // det gick bra
                }
            }
            return 0; // det gick inte bra, det kanske är fullt ?

        }

        //public List<int> Add(int space)  // försök hitta första antal platser bredvid varrandra, returnera platserna
        //{
        //    for (int i = 0; i < Max - 1; i++)
        //    {
        //        if (gar[i] == false)
        //        {
        //            gar[i] = true;
        //            return i; // det gick bra
        //        }
        //    }
        //    return 0; // det gick inte bra, det kanske är fullt ?
        //}



        public bool AddToParkNr(int p)  // parkera på plats
        {
            if (gar[p] == false)
            {
                gar[p] = true;
                return true; // det gick bra
            }
            return false; // det gick inte bra, den är nog upptagen ?

        }

        public bool Remove(int p)
        {
                if (gar[p] == true)
                {
                    gar[p] = false;
                    return true;
                }
            return false; // om den inte var upptagen
        }

        public int Count() // räkna antalet objekt i arrayen
        {
            int x = 0;
            for (int i = 1; i < Max - 1; i++)
            {
                if (gar[i] == true)
                    x++;
            }
            return x;
        }

    }
}