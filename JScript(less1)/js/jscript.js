function bigPict(){
     var w=document.planet.width;
     if (w<302){
      document.planet.width=w+10;
      document.planet.src="img/planet.jpg"
      setTimeout("bigPict()", 1)
     }
    }

function smallPict(){
    document.planet.width = 102;    
}