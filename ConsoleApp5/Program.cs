using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Net;
using System.IO;

namespace ConsoleApp5

{
    // class InfoCoin used for JSON 
    public class InfoCoin
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Coin [] result { get; set; }
    }

    public class Coin
    {
        public string MarketName { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Volume { get; set; }
        public float Last { get; set; }
        public float BaseVolume { get; set; }
        public string TimeStamp { get; set; }
        public float Bid { get; set; }
        public float Ask { get; set; }
        public int OpenBuyOrders { get; set; }
        public int OpenSellOrders { get; set; }
        public float PrevDay { get; set; }
        public string Created { get; set; }
    }

    class Program
    {
        private static void FindCoin(object obj)
        {   // формируем запрос на сайт Bittrex
            string requestUrl = (string)obj;
            WebRequest bittrexApi = WebRequest.Create("https://bittrex.com/api/v1.1/public/getmarketsummaries");
            //получаем ответ в поток
            Stream streamBittrex = bittrexApi.GetResponse().GetResponseStream();
            //Создание класса для десериализации
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(InfoCoin));
            //Десериализация файла JSON. Открытие его, десериализация в переменную infoCoin 
            InfoCoin infoCoin = new InfoCoin();
            infoCoin = (InfoCoin)jsonSerializer.ReadObject(streamBittrex);
            int difBuyAndSell, colCoin = 0;
            foreach (Coin st in infoCoin.result)
                if (st.OpenBuyOrders > st.OpenSellOrders)
                {
                    difBuyAndSell = st.OpenBuyOrders - st.OpenSellOrders;
                    Console.WriteLine("Монета: {0}, Разница: {1}", st.MarketName, difBuyAndSell);
                    colCoin = ++colCoin;

                }
            Console.WriteLine("Количество пар: {0}", colCoin);

        }

        static void Main(string[] args)
        {   //указываем ссылку для запроса
            
            object obj = "https://bittrex.com/api/v1.1/public/getmarketsummaries";
                       
            Console.WriteLine("=>Программа, которая делает POST запрос на сайт Bittrex.");
            Console.WriteLine("=> Получает ответ в виде JSON формата");
            Console.WriteLine("=>Десерализирует JSON формат в класс C#");
            Console.WriteLine("=>Производит вычисления");
            Console.WriteLine("=> Анализ заявок на покупку больше чем на продажу");
            
            Console.WriteLine("=>Выводит эти пары");
            // TimerCallback timerCallback = new TimerCallback(FindCoin);
            // Timer timer = new Timer (timerCallback,null,0,10000);

            //Поиск последовательности, которая растет на покупку
            FindCoin(obj);

            Console.ReadLine();

    
        }

        
    }
}
