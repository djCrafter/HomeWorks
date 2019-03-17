var check = [
    { name: "Чай", quantity: 9, price: 40 },
    { name: "Молоко", quantity: 3, price: 36 },
    { name: "Мороженное", quantity: 4, price: 15 },
    { name: "Йогурт", quantity: 7, price: 26 },
    { name: "Хлеб", quantity: 2, price: 12 },
    { name: "Колбаса", quantity: 3, price: 80 },
    { name: "Печенье", quantity: 8, price: 20 },
    { name: "Соль", quantity: 1, price: 18 },
    { name: "Сок", quantity: 3, price: 36 },
    { name: "Минеральная Вода", quantity: 4, price: 12 }
]



check.calcSum = function() {
    let sum = 0;
    
    this.forEach(function(item){
    sum += (item.price * item.quantity);    
    });
    
    return sum;
}

check.printCheck =  function() {
     this.forEach(function(item) {
   document.write(item.name + " &nbsp; " + item.price +
                 " &nbsp; " + item.quantity * item.price + "<br/>");    
    });
    
    document.write("ИТОГО: " + this.calcSum() + "<br/>");
}

check.mostExpensivePurchase = function(){
    let index = 0;
    let sum = 0;
    
    for(let i = 0; i < this.length; ++i){
       if((this[i].price * this[i].quantity) > sum){
           sum = this[i].price * this[i].quantity;
           index = i;
       }     
}
    
      return this[index];
}

check.averageSum = function() {
    let totalQantity = 0;
    
    this.forEach(function(item){
       totalQantity += item.quantity; 
    });
       
    return (this.calcSum() / totalQantity);
}

var item = check.mostExpensivePurchase();

check.printCheck();
document.write("<br/>Самая дорогая покупка: " + 
               item.name + " &nbsp; " + item.price +
" &nbsp; " + item.quantity * item.price + "<br/>");
document.write("Средняя стоимость одного товара в чеке: " +
              check.averageSum().toFixed(2));

