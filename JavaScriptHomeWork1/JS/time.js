'use strict'

function Time(hours, min, sec){
    if(hours >= 0 && min >= 0 && sec >= 0 &&
      hours < 24 && min < 60 && sec < 60){
        this.hours = hours;
        this.min = min;
        this.sec = sec;
    }
}

Time.prototype.toString = function() {
    var h = this.hours;
    var m = this.min;
    var s = this.sec;
    
    if(h < 10) h = '0' + h;
    if(m < 10) m = '0' + m;
    if(s < 10) s = '0' + s;
              
   return h + ":" + m + ":" + s;
}

Time.prototype.printLn = function() {
    document.write(this + "<br/>");
}

Time.prototype.print = function() {
    document.write(this);
}

Time.prototype.AddHours = function(hours) {     
    if(hours >= 0){
    for(let i = 0; i < hours; ++i){
        this.hours++;
        if(this.hours > 23)
            this.hours = 0;
    }
    }
    else{
        hours = -hours;
    for(let i = 0; i < hours; ++i){
        this.hours--;
        if(this.hours < 0)
            this.hours = 23;
    }
    }
}

Time.prototype.AddMin = function(min) {
    if(min >= 0){
    for(let i = 0; i < min; ++i){
        this.min++;
        if(this.min > 59) {
            this.min = 0;
            this.AddHours(1);
        }
    }  
}
    else{
        min = -min;
        for(let i = 0; i < min; ++i){
         this.min--;
            if(this.min < 0){
                this.min = 59;
                this.AddHours(-1);
            }
        }
    }    
}

Time.prototype.AddSec = function(sec){
    if(sec >= 0){
    for(let i = 0; i < sec; ++i){
        this.sec++;
        if(this.sec > 59) {
            this.sec = 0;
            this.AddMin(1);
        }
    }  
}
    else{
        min = -sec;
        for(let i = 0; i < sec; ++i){
         this.sec--;
            if(this.sec < 0){
                this.sec = 59;
                this.AddMin(-1);
            }
        }
    }    
}

Time.prototype.AddTime = function(hours, min, sec){
    this.AddHours(hours);
    this.AddMin(min);
    this.AddSec(sec);
}

Time.prototype.AddTime = function(objTime){
    this.AddHours(objTime.hours);
    this.AddMin(objTime.min);
    this.AddSec(objTime.sec);
}



var time = new Time(22, 50, 45);
time.print();
document.write(" + ");

var time2 = new Time(3, 12, 15);
time2.print();
document.write(" = ");

time.AddTime(time2);
time.printLn();
