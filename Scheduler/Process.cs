using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    class Process
    {
        private int id;
        private int arrival;
        private int burst;
        private int priority;
        private int start;
        private int end;
        private int remaining;
        private string color;

        //constructor with set initial values
        public Process(int id, int arrival, int burst, int priority)
        {
            this.id = id;
            this.arrival = arrival;
            this.burst = burst;
            this.priority = priority;
            this.remaining = burst;
        }
        
        public Process() { }
        
        public void setID(int id) { this.id = id; }
        public void setArrival(int arrival) { this.arrival = arrival; }
        public void setBurst(int burst) { this.burst = burst; }
        public void setPriority(int priority) { this.priority = priority; }
        public void setStart(int start) { this.start = start; }
        public void setEnd(int end) { this.end = end; }
        public void setRT(int r) { this.remaining = r; }
        public void setColor(string c) { this.color = c; }

        public int getID() { return this.id; }
        public int getArrival() { return this.arrival; }
        public int getBurst() { return this.burst; }
        public int getPriority() { return this.priority; }
        public int getStart() { return this.start; }
        public int getEnd() { return this.end; }
        public int getRT() { return this.remaining; }
        public int getWT() { return this.start - this.arrival ; }
        public string getColor() { return this.color; }
        
    }
}
