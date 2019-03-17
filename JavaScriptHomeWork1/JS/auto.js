'use strict'

function Auto(mark, model, year, averageSpeed){
          this.mark = mark;
          this.model = model;
          this.year = year;
          this.averageSpeed = averageSpeed;                 
}       
       
        Auto.prototype.print =  function()
        {      
            document.write("Mark: " + this.mark + "<br/>");
            document.write("Model: " + this.model + "<br/>");
            document.write("Year: " + this.year + "<br/>");
            document.write("AverageSpeed: " + this.averageSpeed + "km/h <br/><br/>");
        }
        
        Auto.prototype.timeCount = function(distance){
           this.averageSpeed;
            var hoursCount = 0;
            var restСounter = 0;
            
            while(distance > this.averageSpeed){
                hoursCount++;
                restСounter++;
                
                if(restСounter == 4) {
                    restСounter = 0;
                    continue;
                }
                
                distance -= this.averageSpeed;
            }
           
            return hoursCount;
        }
               
        var objAuto = new Auto("Форд", "Фокус", 2018, 150);
        
        objAuto.print();
        
        var distance = 10000;
        
        document.write("The car will travel a given distance beyond " + objAuto.timeCount(distance) + " hours<br/>");               