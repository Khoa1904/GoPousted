using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models.Custom.SellingStatus
{
    public class MtSelngSttusModel : INotifyPropertyChanged
    {
        public string week1;
        public string week2;
        public string week3;
        public string week4;
        public string week5;
        public string week6;
        public string week7;


        public string Week1
        {
            get => week1;
            set
            {
                week1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Week1)));
            }
        }

        public string Week2
        {
            get => week2;
            set
            {
                week2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Week2)));
            }
        }
        public string Week3
        {
            get => week3;
            set
            {
                week3 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Week3)));
            }
        }

        public string Week4
        {
            get => week4;
            set
            {
                week4 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Week4)));
            }
        }
        public string Week5
        {
            get => week5;
            set
            {
                week5 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Week5)));
            }
        }

        public string Week6
        {
            get => week6;
            set
            {
                week6 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Week6)));
            }
        }
        public string Week7
        {
            get => week7;
            set
            {
                week7 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Week7)));
            }
        }


        public string sunday;
        public string monday;
        public string tuesday;
        public string wednesday;
        public string thursday;
        public string friday;
        public string saturday;

        public string Sunday {
            get => sunday;
            set
            {
                sunday = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunday)));
            }
        }

        public string Monday
        {
            get => monday;
            set
            {
                monday = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Monday)));
            }
        }

        public string Tuesday
        {
            get => tuesday;
            set
            {
                tuesday = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tuesday)));
            }
        }

        public string Wednesday
        {
            get => wednesday;
            set
            {
                wednesday = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Wednesday)));
            }
        }

        public string Thursday
        {
            get => thursday;
            set
            {
                thursday = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Thursday)));
            }
        }

        public string Saturday
        {
            get => saturday;
            set
            {
                saturday = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Saturday)));
            }
        }

        public string Friday
        {
            get => friday;
            set
            {
                friday = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Friday)));
            }
        }

        public string total;
        public string Total
        {
            get => total;
            set
            {
                total = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Total)));
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

  
    }
}
