using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleApp1
{
    // Класс подзадач без встроенных функций дополнения
    public class Markdown
    {
        public bool done;
        public string text;

        public Markdown(string text, bool done)
        {
            this.text = text;
            this.done = done;
        }
        public Markdown(string text)
        {
            this.text = text;
            done = false;
        }
    };

    // Просто класс задачи без его дополнения
    public class OneTask
    {
        private Markdown[] tasks_ = new Markdown[100];
        private int Marks;
        public OneTask(int i)
        {
            if (i > 0)
            {
                Tasks_ = new Markdown[i];
                
                size_ = i;
            }
            else
            {
                Tasks_ = new Markdown[1];
                size_ = 1;
            }
            for (int j = 0; j < Tasks_.Length; j++)
                Tasks_[j] = new Markdown(" ");
        }

        public string topic;
        public string text;

        public DateTime time;
        public int size_;

        internal Markdown[] Tasks_ { get => tasks_; set => tasks_ = value; }

    }

    // Наследуемся он класса Задачи для доступа к приватным переменным
    // Это необходимо для работы логики добавления и сортировки задач по времени
    public class Tasks : OneTask {

        public OneTask[] array = new OneTask[100] ;
        public int size1 = 0;

        // Обьявление. Base - От родительского класса
        public Tasks(int i) : base(i)
        {
            if (i > 0)
            {
                array = new OneTask[i];
                for( int j = 0;j < array.Length; j++ )
                {
                    array[j] = new OneTask(1);
                }
            }
            else
            {
                array = new OneTask[1];
                array[0] = new OneTask(1);
            }
        }

        // Автоматическое добавление задачи
        public void Add(string TopicName,
                        string Description,
                        Markdown[] m,
                        DateTime Date)
        {
            // Проверка на длинну для увеличения размера массива

             if (size_ + 1 >= array.Length)
            {
                OneTask[] array_ = new OneTask[array.Length*2];
                for(int i = 0; i < array_.Length; i++)
                {
                    if (i < array.Length)
                        array_[i] = array[i];
                    else
                        array_[i] = new OneTask(10);
                }
                // Создание на 5 больше, ибо это занимает меньше памяти, чем удвоение, как было у нас в примерах реализации у Куренковой
                array = array_;
            }

            if (array[size1] is null)
                array[size1] = new OneTask(10);
            array[size1].topic = TopicName;
            array[size1].text = Description;
            array[size1].Tasks_ = m;
            array[size1].time = Date;

            size1++;
        }

    };

   
  
    

}