using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Scheduler
{
    public partial class Form1 : Form
    {
        int njobs;
        string algorithm;
        int quantum;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
         
            int total_burst = 0;
            njobs = Int32.Parse(text_njobs.Text);
             
            algorithm = ddl_algorithm.Text;
            if (algorithm == "Round Robin")
            {
                if (text_quantum.Text != "")
                {
                    quantum = Int32.Parse(text_quantum.Text);
                }
                else {
                    string message = "Missing Data";
                    string caption = "Error Detected in Input";
                    MessageBox.Show(message, caption);
                    algorithm = "none";
                }
            }
           // we created a list where we'll store the incoming processes data

            List<Process> queue = new List<Process> ();

            for (int i = 1; i < njobs + 1 ; i++)
            {
                Process p = new Process();
                
                Control c = (TextBox)tableLayoutPanel1.GetControlFromPosition(0,i);
                
                if (c.Text != "")
                {
                    p.setID(Int32.Parse(c.Text));
                }
                else 
                {
                    string message = "Missing Data";
                    string caption = "Error Detected in Input";
                    MessageBox.Show(message, caption);
                    algorithm = "none";
                    break;
                }

                c = (TextBox)tableLayoutPanel1.GetControlFromPosition(1,i);
                if (c.Text != "")
                {
                    p.setArrival(Int32.Parse(c.Text));
                }
                else
                {
                    string message = "Missing Data";
                    string caption = "Error Detected in Input";
                    MessageBox.Show(message, caption);
                    algorithm = "none";
                    break;
                }
                
                c = (TextBox)tableLayoutPanel1.GetControlFromPosition(2,i);
                if (c.Text != "")
                {
                    p.setBurst(Int32.Parse(c.Text));
                    p.setRT(Int32.Parse(c.Text));
                    total_burst += Int32.Parse(c.Text);
                }
                else
                {
                    string message = "Missing Data";
                    string caption = "Error Detected in Input";
                    MessageBox.Show(message, caption);
                    algorithm = "none";
                    break;
                }
                if(algorithm == "Priority - Preemptive" || algorithm == "Priority - Nonpreemptive")
                {
                    c = (TextBox)tableLayoutPanel1.GetControlFromPosition(3, i);

                    if (c.Text != "")
                    {
                        p.setPriority(Int32.Parse(c.Text));
                    }
                    else
                    {
                        string message = "Missing Data";
                        string caption = "Error Detected in Input";
                        MessageBox.Show(message, caption);
                        algorithm = "none";
                        break;
                    }
                }
                queue.Add(p);
            }

            //Here, all processes have been added to the queue..
            //FCFS
            if (algorithm == "FCFS") { 

                //Sort according to arrival Time
                list_Sort(queue, "arrival");
                //We have to calculate the start and end of each process
                for (int i = 0; i < queue.Count; i++)
                {
                    if (i == 0)
                    {
                        int start = queue[i].getArrival();
                        int end = start + queue[i].getBurst();
                        queue[i].setStart(start);
                        queue[i].setEnd(end);
                    }
                    else
                    {
                        int start = queue[i - 1].getEnd();
                        int end = start + queue[i].getBurst();
                        queue[i].setStart(start);
                        queue[i].setEnd(end);
                    }
                }
                float sum = 0;
                //Calculate average WT
                for (int i = 0; i < queue.Count; i++) {
                    sum += queue[i].getWT();
                }
                float avgWT = sum / queue.Count;

                    //Start drawing Gantt Chart!!
                    DrawGantt(queue);
                    label_wait.Text = avgWT.ToString();
            }
            else if (algorithm == "SJF - Nonpreemtive")
            {
                list_Sort(queue, "sjf");
                for (int i = 0; i < queue.Count; i++)
                {
                    if (i == 0)
                    {
                        int start = queue[i].getArrival();
                        int end = start + queue[i].getBurst();
                        queue[i].setStart(start);
                        queue[i].setEnd(end);
                    }
                    else
                    {
                        int start = queue[i - 1].getEnd();
                        int end = start + queue[i].getBurst();
                        queue[i].setStart(start);
                        queue[i].setEnd(end);
                    }
                }
                float sum = 0;
                //Calculate average WT
                for (int i = 0; i < queue.Count; i++)
                {
                    sum += queue[i].getWT();
                }
                float avgWT = sum / queue.Count;

                //Start drawing Gantt Chart!!
                DrawGantt(queue);
                label_wait.Text = avgWT.ToString();
            }
            else if (algorithm == "Priority - Nonpreemptive")
            {
                list_Sort(queue, "priority");
                for (int i = 0; i < queue.Count; i++)
                {
                    if (i == 0)
                    {
                        int start = queue[i].getArrival();
                        int end = start + queue[i].getBurst();
                        queue[i].setStart(start);
                        queue[i].setEnd(end);
                    }
                    else
                    {
                        int start = queue[i - 1].getEnd();
                        int end = start + queue[i].getBurst();
                        queue[i].setStart(start);
                        queue[i].setEnd(end);
                    }
                }
                float sum = 0;
                //Calculate average WT
                for (int i = 0; i < queue.Count; i++)
                {
                    sum += queue[i].getWT();
                }
                float avgWT = sum / queue.Count;

                //Start drawing Gantt Chart!!
                DrawGantt(queue);
                label_wait.Text = avgWT.ToString();  
            }
            else if (algorithm == "Round Robin") {
                quantum =Int32.Parse(text_quantum.Text);
                
                //Sort according to arrival Time
                list_Sort(queue, "arrival");

                int t = 0;
                List<Process> readyQ = new List<Process>();

                while (t < total_burst) {
                    for (int i = 0; i < queue.Count; i++) {
                        if (queue[i].getRT() > 0 && queue[i].getArrival() <= t) { //there is still remaining time
                            if (queue[i].getRT() < quantum)
                            { //remaining time is less than quantum

                                Process p = new Process();
                                p.setID(queue[i].getID());
                                p.setStart(t);
                                p.setEnd(t + queue[i].getRT());

                                readyQ.Add(p);

                                if (queue[i].getRT() == queue[i].getBurst()) { //y3ne lessa hatebda2
                                    queue[i].setStart(t);
                                }
                                queue[i].setEnd(t + queue[i].getRT()); //updates end of original process

                                t += queue[i].getRT();
                                queue[i].setRT(0);
                            }
                            else
                            { //remaining time is greater than quantum 
                                Process p = new Process();
                                p.setID(queue[i].getID());
                                p.setStart(t);
                                p.setEnd(t + quantum);

                                readyQ.Add(p);

                                if (queue[i].getRT() == queue[i].getBurst())
                                { //y3ne lessa hatebda2
                                    queue[i].setStart(t);
                                }
                                queue[i].setEnd(t + queue[i].getRT()); //updates end of original process

                                t += quantum;
                                int r = queue[i].getRT() - quantum;
                                queue[i].setRT(r);
                            }   
                        }
                    }
                }

                float sum = 0;
                for (int i = 0; i < queue.Count; i++) { 
                    sum+= (queue[i].getEnd()-queue[i].getArrival() - queue[i].getBurst()); //wt of each process = tat - bt // tat = end - arrival                
                }
                float avgWT = sum / (queue.Count);
                    
                //Draw Gantt chart and update Waiting Time
                DrawGantt(readyQ);    
                label_wait.Text = avgWT.ToString(); 

            }
            else if (algorithm == "SJF - Preemptive") {
                list_Sort(queue, "sjf");
                int t = 0;
                List<Process> readyQ = new List<Process>();
                //awel wa7da eshtaghalet heya awel process fel sorted
                int running_index = 0;
                queue[running_index].setStart(0);

                while (t < total_burst) {
                    t++;

                    int currRT = queue[running_index].getRT();
                    queue[running_index].setRT(currRT - 1);

                    if(queue[running_index].getRT()==0){

                        queue[running_index].setEnd(t);

                        Process p = new Process();
                        p.setID(queue[running_index].getID());
                        p.setStart(queue[running_index].getStart());
                        p.setEnd(queue[running_index].getEnd());

                        readyQ.Add(p);
                        //3ayez akhalli el running ay wa7da shaghala delwa2te , w hazabat law heya el 3ayzenha wala la2 fel for loop
                        for (int i = 0; i < queue.Count; i++) {
                            if (queue[i].getRT() != 0)
                            {
                                running_index = i;
                                queue[running_index].setStart(t);
                                break;
                            }
                        }
                    }
                    for (int i = 0; i < queue.Count; i++) {
                        //y3ne law feeh wa7da gatt at t(zay timer), wel burst bta3ha a2al mn el burst bta3 el running
                        if (queue[i].getArrival() <= t && queue[i].getRT() < queue[running_index].getRT() && queue[i].getRT() !=0 )
                        { 
                            //3ayza awa2af el running
                            //ha-set el end bta3ha == t
                            //wa3mel process gdeeda leeha start w end zayaha
                            //wa2ool en el running index = i
                            queue[running_index].setEnd(t);

                            if (queue[running_index].getEnd() != queue[running_index].getStart())
                            {
                                Process p = new Process();
                                p.setID(queue[running_index].getID());
                                p.setStart(queue[running_index].getStart());
                                p.setEnd(queue[running_index].getEnd());

                                readyQ.Add(p);
                            }
                                //2olt en el running now heya el process el 7a2a2et el condn.
                                //w ha-set el start bta3ha b t
                                running_index = i;
                                queue[running_index].setStart(t);   
                        }
                    }   
                }
                float sum = 0;
                for (int i = 0; i < queue.Count; i++)
                {
                    sum += (queue[i].getEnd() - queue[i].getArrival() - queue[i].getBurst()); //wt of each process = tat - bt // tat = end - arrival                
                }
                float avgWT = sum / (queue.Count);
                DrawGantt(readyQ);
                label_wait.Text = avgWT.ToString(); 
            
            }
            else if (algorithm == "Priority - Preemptive") {
                list_Sort(queue, "priority");
                int t = 0;
                List<Process> readyQ = new List<Process>();
                int running_index = 0;
                queue[running_index].setStart(0);

                while (t < total_burst)
                {
                    t++;
                    int currRT = queue[running_index].getRT();
                    queue[running_index].setRT(currRT - 1);
                    if (queue[running_index].getRT() == 0)
                    {
                        queue[running_index].setEnd(t);
                        Process p = new Process();
                        p.setID(queue[running_index].getID());
                        p.setStart(queue[running_index].getStart());
                        p.setEnd(queue[running_index].getEnd());
                        readyQ.Add(p);
                        for (int i = 0; i < queue.Count; i++)
                        {
                            if (queue[i].getRT() != 0)
                            {
                                running_index = i;
                                queue[running_index].setStart(t);
                                break;
                            }
                        }
                    }
                    for (int i = 0; i < queue.Count; i++)
                    {
                        //y3ne law feeh wa7da gatt at t(zay timer), wel burst bta3ha a2al mn el burst bta3 el running
                        if (queue[i].getArrival() <= t && queue[i].getPriority() < queue[running_index].getPriority() && queue[i].getRT() != 0)
                        {
                            queue[running_index].setEnd(t);

                            if (queue[running_index].getEnd() != queue[running_index].getStart())
                            {
                                Process p = new Process();
                                p.setID(queue[running_index].getID());
                                p.setStart(queue[running_index].getStart());
                                p.setEnd(queue[running_index].getEnd());

                                readyQ.Add(p);
                            }
                            running_index = i;
                            queue[running_index].setStart(t);
                        }
                    }
                }
                float sum = 0;
                for (int i = 0; i < queue.Count; i++)
                {
                    sum += (queue[i].getEnd() - queue[i].getArrival() - queue[i].getBurst()); //wt of each process = tat - bt // tat = end - arrival                
                }
                float avgWT = sum / (queue.Count);
                DrawGantt(readyQ);
                label_wait.Text = avgWT.ToString(); 
            }
        }
        
        
        
        private void list_Sort(List<Process> list, string alg) {
            if(alg == "arrival"){
                //sorts processes according to their arrival time
                for (int i = 0; i < list.Count - 1; i++) {
                    int minindex = i;
                    for (int j = i + 1; j < list.Count; j++) {
                       
                        if (list[j].getArrival() < list[minindex].getArrival()) {
                            minindex = j;
                        }
                    }
                    if (minindex != i) {
                        Process temp = list[i];
                        list[i] = list[minindex];
                        list[minindex] = temp;
                    }
                    
                }
            }
            else if (alg == "sjf") { 
                //sort processesaccording to their arrival then burst
                int t = 0;
                for (int i = 0; i < list.Count; i++) {
                    for (int j = i + 1; j < list.Count; j++) {
                        if (list[j].getArrival() <= t) {
                            if (list[j].getBurst() < list[i].getBurst()) { 
                                //swap
                                Process temp = list[i];
                                list[i] = list[j];
                                list[j] = temp;
                            }
                        }
                    }
                    t += list[i].getBurst();
                }
            }
            else if (alg == "priority")
            {
                //sort processesaccording to their arrival then burst
                int t = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = i + 1; j < list.Count; j++)
                    {
                        if (list[j].getArrival() <= t)
                        {
                            if (list[j].getPriority() < list[i].getPriority())
                            {
                                //swap
                                Process temp = list[i];
                                list[i] = list[j];
                                list[j] = temp;
                            }
                        }
                    }
                    t += list[i].getBurst();
                }
            }


        }

        private void DrawGantt(List<Process> proc)
        {
            chart1.ChartAreas[0].AxisY.IsStartedFromZero = true;

            //initialize a legend with some settings
            chart1.Legends.Clear();
            chart1.Legends.Add("Timespans");
            chart1.Legends[0].LegendStyle = LegendStyle.Table;
            chart1.Legends[0].Docking = Docking.Bottom;
            chart1.Legends[0].Alignment = StringAlignment.Center;
            chart1.Legends[0].Title = "Timespans";
            chart1.Legends[0].BorderColor = Color.Black;

            chart1.Series.Clear();
            string seriesname;
            //adding the bars with some settings
            for (int i = 0; i < proc.Count; i++)
            {
                seriesname = Convert.ToString("P" + proc[i].getID() + "::" + proc[i].getStart() + " - " + proc[i].getEnd());
                chart1.Series.Add(seriesname);
                chart1.Series[seriesname].ChartType = SeriesChartType.RangeBar;
                chart1.Series[seriesname].YValuesPerPoint = 2;
                chart1.Series[seriesname].Points.AddXY("Timeline", proc[i].getStart(), proc[i].getEnd());
                chart1.Series[seriesname]["DrawSideBySide"] = "false";
                chart1.Series[seriesname].BorderColor = Color.Black;
                chart1.Series[seriesname].ToolTip = seriesname;
                chart1.Series[seriesname].Label = Convert.ToString("P" + proc[i].getID());
                
            }
        }

        private void ddl_algorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            algorithm = ddl_algorithm.Text;
            if (algorithm == "FCFS" || algorithm == "SJF - Preemptive" || algorithm == "SJF - Nonpreemtive" || algorithm == "Priority - Preemptive" || algorithm == "Priority - Nonpreemptive") {
                text_quantum.Enabled = false;
            }
            if (algorithm == "FCFS" || algorithm == "SJF - Preemptive" || algorithm == "SJF - Nonpreemtive" || algorithm == "Round Robin") {
                for (int i = 1; i < tableLayoutPanel1.RowCount; i++)
                {
                    Control c = (TextBox)tableLayoutPanel1.GetControlFromPosition(3, i);
                    c.Enabled = false;
                }
            }
           if(algorithm == "Priority - Preemptive" || algorithm == "Priority - Nonpreemptive"){
            for (int i = 1; i < tableLayoutPanel1.RowCount; i++)
                {
                    Control c = (TextBox)tableLayoutPanel1.GetControlFromPosition(3, i);
                    c.Enabled = true;
                }
           }
           if (algorithm == "Round Robin") {
               text_quantum.Enabled = true;
           }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                TextBox c = new TextBox();
                tableLayoutPanel1.Controls.Add(c, i, tableLayoutPanel1.RowCount);
            }
        }
    
    }
}