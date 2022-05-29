using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppFinalStruct
{
    internal class Program
    {
        static int globalPhoneId = 0;
        const string globalBinFileName = "phones.bin";


        enum DevelopmentCountry
        {
            CHINA,
            USA,
            RUSSIA,
            OTHER
        }

        [Serializable]
        struct Phone
        {
            public int Id;
            public string Model;
            public int Price;
            public int DevelopmentYear;
            public DevelopmentCountry DevelopmentCountry;
        }


        static Phone[] CreateEmptyPhoneArray()
        {
            return new Phone[0];
        }

        static Phone[] AddNewPhoneToEnd(Phone[] originalPhones, Phone insertPhone)
        {
            Phone[] newPhones = new Phone[originalPhones.Length + 1];

            for (int i = 0; i < originalPhones.Length; i++)
            {
                newPhones[i] = originalPhones[i];
            }

            newPhones[newPhones.Length - 1] = insertPhone;

            return newPhones;
        }

        static Phone CreateNewPhone()
        {
            Phone phone = new Phone();

            globalPhoneId++;

            phone.Id = globalPhoneId;

            Console.Write("введите модель телефона: ");
            phone.Model = Console.ReadLine();

            Console.Write("введите цену телефона: ");
            phone.Price = int.Parse(Console.ReadLine());

            Console.Write("введите год производства: ");
            phone.DevelopmentYear = int.Parse(Console.ReadLine());

            Console.Write("введите страну производитель (0-CHINA,1-USA,2-RUSSIA,3-OTHER): ");
            phone.DevelopmentCountry = (DevelopmentCountry)int.Parse(Console.ReadLine());

            return phone;
        }

        static void ShowTableHeader()
        {
            Console.WriteLine("{0,-3}{1,-10}{2,-7}{3,-17}{4,-20}", "ИД", "Модель", "Цена", "Год производства", "Страна производитель");
        }

        static void ShowPhone(Phone phone)
        {
            Console.WriteLine("{0,-3}{1,-10}{2,-7}{3,-17}{4,-20}", phone.Id, phone.Model, phone.Price, phone.DevelopmentYear, phone.DevelopmentCountry);
        }

        static void ShowPhones(Phone[] phones)
        {
            ShowTableHeader();
            if (phones.Length == 0)
            {
                Console.WriteLine("Список пуст");
            }
            else
            {
                for (int i = 0; i < phones.Length; i++)
                {
                    ShowPhone(phones[i]);
                }
            }
        }

        static void ShowSeparator()
        {
            Console.WriteLine("---------------");
        }

        static void ShowMenu()
        {
            Console.WriteLine("Меню:");
            Console.WriteLine("1.Добавить элемент в конец списка");
            Console.WriteLine("2.Сохранить список в bin файл");
            Console.WriteLine("3.Загрузить список из bin файла");
            Console.WriteLine("4.Отсортировать массив");
            Console.WriteLine("0.Выход");
        }

        static int ChooseAction()
        {
            Console.Write("Выберите пункт меню: ");
            return int.Parse(Console.ReadLine());
        }

        static void SavePhonesToBinFile(Phone[] phones)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            FileStream fileStream = new FileStream(globalBinFileName, FileMode.OpenOrCreate);

            binaryFormatter.Serialize(fileStream, phones);

            fileStream.Close();
        }

        static Phone[] LoadPhonesFromBinFile()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            FileStream fileStream = new FileStream(globalBinFileName, FileMode.Open);

            Phone[] phones = (Phone[])binaryFormatter.Deserialize(fileStream);

            fileStream.Close();

            return phones;
        }

        static int GetLastPhoneId(Phone[] phones)
        {
            int maxId = phones[0].Id;

            for (int i = 0; i < phones.Length; i++)
            {
                if (phones[i].Id > maxId)
                {
                    maxId = phones[i].Id;
                }
            }

            return maxId;
        }

        static int CompareTwoPhones(Phone phone1, Phone phone2)
        {
            if (phone1.Price > phone2.Price)
            {
                return -1;
            }
            else if (phone1.Price < phone2.Price)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        static void SortPhonesByPrice(Phone[] phones)
        {
            Array.Sort(phones, CompareTwoPhones);
        }

        static void Main(string[] args)
        {
            Phone[] phones = CreateEmptyPhoneArray();

            while (true)
            {
                Console.Clear();
                ShowPhones(phones);
                ShowSeparator();

                ShowMenu();
                int action = ChooseAction();

                switch (action)
                {
                    case 1:
                        {
                            Phone phone = CreateNewPhone();
                            phones = AddNewPhoneToEnd(phones, phone);
                        }
                        break;
                    case 2:
                        {
                            SavePhonesToBinFile(phones);
                        }
                        break;
                    case 3:
                        {
                            phones = LoadPhonesFromBinFile();
                            globalPhoneId = GetLastPhoneId(phones);
                        }
                        break;
                    case 4:
                        {
                            SortPhonesByPrice(phones);
                        }
                        break;

                    case 0:
                        {
                            Environment.Exit(0);
                        }
                        break;
                }
            }





        }
    }
}
