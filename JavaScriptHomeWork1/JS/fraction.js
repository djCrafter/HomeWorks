'use strict'

function Fraction(numerator, denumerator){
    this.numerator = numerator;
    this.denumerator = denumerator;
}

Fraction.prototype.toString = function(){
    return this.numerator + "/" + this.denumerator;
}
   

function Addition(fractA, fractB){   
    var temp = new Fraction();
    var x = new Fraction();
    var y = new Fraction();
    
 if(fractA.denumerator == fractB.denumerator){
    temp.denumerator = fractA.denumerator;
    temp.numerator = fractA.numerator + fractB.numerator;
    return temp;
 }
    else{
    x.numerator = fractA.numerator * fractB.denumerator;
    x.denumerator = fractA.denumerator * fractB.denumerator;
    y.numerator = fractB.numerator * fractA.denumerator;
    y.denumerator = fractB.denumerator * fractA.denumerator;
    temp.numerator = x.numerator + y.numerator;
    temp.denumerator = x.denumerator;
    return temp;
    }
}


function Substraction(fractA, fractB){
    var temp = new Fraction();
    var x = new Fraction();
    var y = new Fraction();
    
    if(fractA.denumerator == fractB.denumerator){
    temp.denumerator = fractA.denumerator;
    temp.numerator = fractA.numerator - fractB.numerator;
    return temp;
 }
    else{
        x.numerator = fractA.numerator * fractB.denumerator;
    x.denumerator = fractA.denumerator * fractB.denumerator;
    y.numerator = fractB.numerator * fractA.denumerator;
    y.denumerator = fractB.denumerator * fractA.denumerator;
    temp.numerator = x.numerator - y.numerator;
    temp.denumerator = x.denumerator;
        
    if(temp.numerator < 0) temp.numerator = -temp.numerator;
    if(temp.denumerator < 0) temp.denumerator = -temp.denumerator; 
        
    return temp;
    }
}

function Multiplication(fractionA, fractionB){
   var temp = new Fraction();
   temp.numerator = fractionA.numerator * fractionB.numerator;
   temp.denumerator = fractionA.denumerator * fractionB.denumerator;
   return temp;
}

function Division(fractionA, fractionB){
    
    var a = new Fraction();
    var b = new Fraction();
    
    b.numerator = fractionB.numerator;
    b.denumerator = fractionB.denumerator;
    
    a.denumerator = fractionA.denumerator;
    a.numerator = fractionA.numerator;
    
    var t = b.numerator; 
    b.numerator = b.denumerator;
    b.denumerator = t;
    
    return Multiplication(a, b);
}



function Reduction(fract){   
    function NOD(x, y){
        if(y == 0) return x;
        return NOD(y, x % y);
    }
    
    var nod = NOD(fract.numerator, fract.denumerator);
    
    var temp = new Fraction();
    temp.numerator = fract.numerator;
    temp.denumerator = fract.denumerator;
    temp.numerator /= nod;
    temp.denumerator /= nod;
    
    return temp;
}

var A = new Fraction(1, 4);
var B = new Fraction(2, 3);

document.write(A + " + " + B + " = ");
document.write(Reduction(Addition(A, B)) + "<br/>");

document.write(A + " - " + B + " = ");
document.write(Reduction(Substraction(A,B)) + "<br/>");

document.write(A + " * " + B + " = ");
document.write(Reduction(Multiplication(A,B)) + "<br/>");

document.write(A + " / " + B + " = ");
document.write(Reduction(Division(A,B)) + "<br/>");





