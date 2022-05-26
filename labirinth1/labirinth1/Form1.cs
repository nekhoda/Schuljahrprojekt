using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace labirinth1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int[,] a = new int[11, 11];//Масив а відтворює лабіринт
        String labirint = "labirint.txt";//Файл, що містить цей масив.
        String results = "results.txt";//Файл для збереження результатів
        /* {
           { 0,0,0,0,0,0,0,0,0,0,0},
           { 0,0,1,0,0,0,0,0,0,0,0},
           { 0,1,1,0,1,0,0,1,1,1,0},
           { 0,0,1,1,1,1,1,1,0,1,0},
           { 0,0,0,0,1,0,1,0,0,1,0},
           { 0,0,1,0,1,1,1,1,0,1,0},
           { 0,0,1,1,1,0,0,0,0,1,0},
           { 0,0,0,0,1,1,0,1,0,1,0},
           { 0,0,1,0,0,1,1,1,0,1,0},
           { 0,0,1,1,1,1,0,0,1,1,0},
           { 0,0,0,0,0,0,0,0,0,0,0},
       };Заповнюємо лабірнт стінами і проходами, де:
                     * "0" - це стіна;
                     * "1" - це прохід;
                     */
        int i1;
        int j1;//i1,j1-координати входу.
        int i2 = 8;
        int j2 = 9;//i2,j2-координати виходу.
        int c = 0;//Лічильник кроків.
        int tupik = 0;//Лічильник тупіків.
        int way = 0;//Лічильник шляхів.
        int minc = 99999;//Для обчислення найкоротшого шляху.
        int maxc = -99999;//Для обчислення найдовшого шляху.
        int[,] l;//Для запам'ятовування шляху.
        int[,] minl;//Для запам'ятовування мінімального шляху.
        int[,] maxl;//Для запам'ятовування максимального шляху.
        public void find(int x, int y)
        {
            c++;//Зроблено крок.
            l[c, 0] = x;
            l[c, 1] = y;//Запам'ятовуємо координати кожного кроку, щоб потім вивести шлях.
            if ((x == i2) && (y == j2))//Перевіряємо поточну позицію в лабіринті: якщо це вихід, виходимо, якщо ні - рухаємося далі.
            {
                StreamWriter w = new StreamWriter(results,true);//Створюємо засіб запису до файлу.
                way++;
                label7.Text = "ВИХІД Є";
                listBox1.Items.Add("Вихід номер " + way);
                w.WriteLine("Вихід номер " + way);
                for (int i = 1; i <= c; i++)
                {
                    listBox1.Items.Add("x:" + l[i, 0] + " " + "y:" + l[i, 1]);//Виводимо по кроках шлях.
                    w.WriteLine("x:" + l[i, 0] + " " + "y:" + l[i, 1]);
                    dataGridView1.Rows[l[i, 1]].Cells[l[i, 0]].Style.BackColor=Color.FromArgb(200 - way * 20, 100+way*10,50 + way * 20);/*Додаємо "way",
                    щоб у різних шляхів були різні кольори.*/

                }
                listBox4.Items.Add("Довжина шляху номер " + way+" : "+c);
                w.WriteLine("Довжина шляху номер " + way + " : " + c);
                w.Close();
                if (c < minc)
                {
                    minc = c;
                    for (int i = 1; i <= minc; i++)//Заповнюємо масив, де зберігається найкоротший шлях.
                    {
                        minl[i, 0] = l[i, 0];
                        minl[i, 1] = l[i, 1];
                    }
                }
                if (c > maxc)
                {
                    maxc = c;
                    for (int i = 1; i <= maxc; i++)//Заповнюємо масив, де зберігається найдовший шлях.
                    {
                        maxl[i, 0] = l[i, 0];
                        maxl[i, 1] = l[i, 1];
                    }
                }
            }
            int k = 0;//За допомогою k визначаємо, скільки навколо поточної позиції стінок. Якщо їх 3, це тупік.
            a[x, y] = 2;//Далі ми позначаємо цю точку "2". Це значить, що ми тут вже були.
            if (a[x + 1, y] == 1) find(x + 1, y);
            else
            {
                if (a[x + 1, y] == 0) k++;
            }
            if (a[x - 1, y] == 1) find(x - 1, y);
            else
            {
                if (a[x - 1, y] == 0) k++;
            }
            if (a[x, y - 1] == 1) find(x, y - 1);
            else
            {
                if (a[x, y-1] == 0) k++;
            }
            if (a[x, y + 1] == 1) find(x, y + 1);
            else
            {
                if (a[x, y+1] == 0) k++;
            }
            if (k == 3)
            {
                tupik++;
            }
            a[x, y] = 1;
            c--;//віднімає крок
        }
        private void button1_Click_1(object sender, EventArgs e)/*Натисканням першої кнопки копіюємо лабіринт із текстового файлу до масиву "а"
та виводимо його на екран.*/
        {
            i1 = 2;
            j1 = 1;
            int k = -1;
            String s1;
            StreamReader str = new StreamReader(labirint); //*Створюємо засіб читання з файлу, відкриваємо файл для читання з нього.
            while ((s1 = str.ReadLine()) != null)//Доки не дійшли до кінця файлу, читаємо рядки з нього.
            {
                k++;
               for (int i = 0; i < s1.Length; i++)
                {
                    a[i, k] = Convert.ToInt16(s1[i])-48;//Заповнюємо масив "а" лабирінтом.
                }             
            }
            str.Close();//закриваємо наш файл
            dataGridView1.ColumnCount = 11;//Задаємо кількість колонок.
            dataGridView1.RowCount = 11;//Задаємо кількість стовпців.
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    dataGridView1.Rows[j].Cells[i].Value = a[i, j];//Заповнюємо  dataGridView1.
                }
            }
            for (int i = 0; i < 11; i++)
            {
                int j = 0;
                dataGridView1.Rows[j].Cells[i].Value = i;
            }
            for (int j = 0; j < 11; j++)
            {
                int i = 0;
                dataGridView1.Rows[j].Cells[i].Value = j;
            }//Нумерація рядків і стовпчиків.      
            dataGridView1.Rows[j1].Cells[i1].Value = "S";//S- місце старту.
            dataGridView1.Rows[j2].Cells[i2].Value = "F";//F- місце фінішу.
        }     
        private void button2_Click(object sender, EventArgs e)//Натиснувши другу кнопку, запускаємо рух по лабіринту.
        {
            dataGridView1.Rows[j1].Cells[i1].Value = "1";//Стираємо минулий початок, якщо він був.
            i1 = 2;
            j1 = 1;//Координати входу до лабіринту за замовчуванням.       
           dataGridView1.Rows[j1].Cells[i1].Value = "S";
            l = new int[11 * 11, 2];
            minl = new int[11 * 11, 2];
            maxl = new int[11 * 11, 2];//Заповнюємо масиви для виходів.
            label7.Text = "ВИХОДУ НЕМАЄ";/*Спочатку пишемо що виходу немає (перед рядком із процедурою  find).
           Якщо find знайде шлях крізь лабіринт, напис буде змінено.*/
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.White;//Обнуляємо замальовані комірки.
                }
            }
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            minc = 999999;
            maxc = -99999;
            c = 0;
            tupik = 0;
            way = 0;
            StreamWriter nul = new StreamWriter(results);//Чистимо файл із результатами перед запуском проекту на виконання.
            nul.Close();//Обнулюємо всі лічильники перед початком роботи процедури  find.
            find(i1, j1);
            StreamWriter SW = new StreamWriter(results, true);//Створюємо засіб запису до файлу.
            SW.WriteLine("Найкоротший шлях");
            for (int i = 1; i <= minc; i++)
            {
                listBox2.Items.Add("x:" + minl[i, 0] + " " + "y:" + minl[i, 1]);//Виводимо найкоротший шлях.
                SW.WriteLine("x:" + minl[i, 0] + " " + "y:" + minl[i, 1]);
            }
            SW.WriteLine("Найдовший шлях");
            for (int i = 1; i <= maxc; i++)
            {
                listBox3.Items.Add("x:" + maxl[i, 0] + " " + "y:" + maxl[i, 1]);//Виводимо найдовший шлях.
                SW.WriteLine("x:" + maxl[i, 0] + " " + "y:" + maxl[i, 1]);
            }       
            listBox4.Items.Add("Довжина найкоротшого виходу " + minc);
            listBox4.Items.Add("Довжина найдовшого виходу " + maxc);
            listBox4.Items.Add("Кількість виходів у лабіринті " + way);
            listBox4.Items.Add("Кількість тупіків у лабіринті " + tupik);
            SW.WriteLine("Довжина найкоротшого виходу " + minc);
            SW.WriteLine("Довжина найдовшого виходу " + maxc);
            SW.WriteLine("Кількість виходів у лабіринті " + way);
            SW.WriteLine("Кількість тупіків у лабіринті " + tupik);
            SW.Close();
        }
        private void button3_Click_1(object sender, EventArgs e)/*Натиснувши третю кнопку, проходимо лабіринт зі стартової позиції,
        яка може бути розташована в довільному місці лабіринту.*/
        {
            dataGridView1.Rows[j1].Cells[i1].Value = "1";//Стираємо минулий початок, якщо він був.
            i1 = int.Parse(textBox1.Text);//Вводимо першу координату.
            j1 = int.Parse(textBox2.Text);//Вводимо другу координату.
            if (a[i1, j1] == 0)//Важливо, щоб вхід було встановлено не на стіні!
            {
                MessageBox.Show("Це стіна!");
            }
            else
            {
                dataGridView1.Rows[1].Cells[2].Value = 1;
                dataGridView1.Rows[j1].Cells[i1].Value = "S";
                l = new int[11 * 11, 2];
                minl = new int[11 * 11, 2];
                maxl = new int[11 * 11, 2];//заповнюємо масиви для виходів.
                label7.Text = "ВИХОДУ НЕМАЄ";/*спочатку пишемо що виходу немає, спеціально перед рядком з процедурою  find:
            якщо процедура знайде шлях, напис буде змінено.*/
                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 11; j++)
                    {
                        dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.White;//Обнулюємо замальовані комірки.
                    }
                }
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                listBox3.Items.Clear();
                listBox4.Items.Clear();
                minc = 999999;
                maxc = -99999;
                c = 0;
                tupik = 0;
                way = 0;
                StreamWriter nul = new StreamWriter(results);//Чистимо файл із результатами перед застосуванням програми.
                nul.Close();//Обнулення всіх лічильників перед початком роботи процедури  find.
                find(i1, j1);
                StreamWriter SW = new StreamWriter(results,true);//Створюємо засіб запису до файлу.
                SW.WriteLine("Найкоротший шлях");
                for (int i = 1; i <= minc; i++)
                {
                    listBox2.Items.Add("x:" + minl[i, 0] + " " + "y:" + minl[i, 1]);//Виводимо найкоротший шлях.
                    SW.WriteLine("x:" + minl[i, 0] + " " + "y:" + minl[i, 1]);
                }
                SW.WriteLine("Найдовший шлях");
                for (int i = 1; i <= maxc; i++)
                {
                    listBox3.Items.Add("x:" + maxl[i, 0] + " " + "y:" + maxl[i, 1]);//Виводимо найдовший шлях.
                    SW.WriteLine("x:" + maxl[i, 0] + " " + "y:" + maxl[i, 1]);
                }               
                listBox4.Items.Add("Довжина найкоротшого виходу " + minc);
                listBox4.Items.Add("Довжина найдовшого виходу " + maxc);
                listBox4.Items.Add("Кількість виходів у лабіринті " + way);
                listBox4.Items.Add("Кількість тупіків у лабіринті " + tupik);
                SW.WriteLine("Довжина найкоротшого виходу " + minc);
                SW.WriteLine("Довжина найдовшого виходу " + maxc);
                SW.WriteLine("Кількість виходів у лабіринті " + way);
                SW.WriteLine("Кількість тупіків у лабіринті " + tupik);
                SW.Close();
            }
         }
         private void button4_Click_1(object sender, EventArgs e)//Натиснувши четверту кнопку, закриваємо проект.
        {  
             Application.Exit();
        }
    }
}