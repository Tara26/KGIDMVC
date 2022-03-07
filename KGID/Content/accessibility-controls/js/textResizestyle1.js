 var pagestyleCookieExpires = 365;  var pagestyleCookieDomain ="index.html";
  var pagestylePath = "sites/all/modules/pagestyle";
  var pagestyleCurrent = "standard";
 var text_resize_scope = "content-inner";
    var text_resize_minimum = "12";
    var text_resize_maximum = "25";
    var text_resize_line_height_allow = false;
    var text_resize_line_height_min = 16;
    var text_resize_line_height_max = 36;
$(document).ready(function(){
    var schh = $.cookie("sche");
 
var chngtextsize= $.cookie("chngtext");
var chngtextspace= $.cookie("textspace"); 


  // Reset Font Size
  var originalFontSize = $('body').css('font-size');
  originalFontSize = '13px';
  $(".resetFont").click(function(){
  $.cookie("chngtext", "2");
  $('body').css('font-size', originalFontSize);
  //alert($('#wrapper').css('font-size'));
   //location.reload();
     
  });
  // Increase Font Size
  $(".increaseFont").click(function(){
    $.cookie("chngtext", "1");
  var currentFontSize = $('body').css('font-size');
 	var currentFontSizeNum = parseFloat(currentFontSize, 10);
  var newFontSize = currentFontSizeNum*1.2;
	if ( newFontSize < 16 ) {
	$('body').css('font-size', newFontSize+"px");
  	}
  // alert($('#wrapper').css('font-size')); 
  return false;
  });
  // Decrease Font Size
  $(".decreaseFont").click(function(){
  $.cookie("chngtext", "3");
  var currentFontSize = $('body').css('font-size');
 	var currentFontSizeNum = parseFloat(currentFontSize, 10);
	var newFontSize = currentFontSizeNum*0.8;
  if ( newFontSize > 8 ) {
	$('body').css('font-size', newFontSize);
  //alert($('#wrapper').css('font-size'));
 	}
   //alert($('#wrapper').css('font-size'));
	return false;
  });
 
   $(".yellowblue").click(function(){
    yellowblue();     
  });
  
    $(".yellowblack").click(function(){
  yellowblack();    
  });
  
     $(".fushblack").click(function(){
  fushblack();     
  });
  
   $(".standards").click(function(){
   $.cookie("sche", "0");
   location.reload();
   
       });
       
     $(".defaultspace").click(function(){
   $.cookie("textspace","0"); 
     $("body").css("letter-spacing", "0px");
       });
     
     $(".wider").click(function(){
   $.cookie("textspace","1"); 
   $("body").css("letter-spacing", "1px");
       });
       
       
     $(".widest").click(function(){
   $.cookie("textspace","2"); 
   $("body").css("letter-spacing", "2px");
       });        
       
       
 

if (schh == '')
{
if (i == 0) {location.reload(); i = 1;}
}

if (schh == 2){
yellowblue(); 
}
if (schh == 1){
yellowblack(); 
}

if (schh == 3)
{ 
fushblack();
}

if (chngtextsize == ''){ 
     if (j == 0) {location.reload(); j = 1;} 
    }
if (chngtextsize == 0){ 
     $("body").css("font-size", "17.2px"); 
    }
  if (chngtextsize == 1){ 
     $("body").css("font-size", "14.4px"); 
    }
  if (chngtextsize == 2){ 
     $("body").css("font-size", "12px"); 
    }
  if (chngtextsize == 3){ 
     $("body").css("font-size", "9.8px"); 
    }
  if (chngtextsize == 4){ 
     $("body").css("font-size", "7.7px"); 
    }
    
  if (chngtextspace == ''){ 
     if (k == 0) {location.reload(); k = 1;}
    } 
  if(chngtextspace == 0) {
     $("body").css("letter-spacing", "0px"); 
    }
  if(chngtextspace == 1) {
     $("body").css("letter-spacing", "1px"); 
    }
  if(chngtextspace == 2) {
     $("body").css("letter-spacing", "2px"); 
  }   

function yellowblue()
{
$.cookie("sche", "2");
$("body, *").css("background", "blue");
$("body, *").css("color", "yellow");
$(".bicons48 div.block-icon").css("height","0px");
}

function yellowblack()
{
 
$.cookie("sche", "1");
$("body, *").css("background", "black");
$("body, *").css("color", "yellow");
$(".bicons48 div.block-icon").css("height","0px");
}

function fushblack()
{
$.cookie("sche", "3");
$("body, *").css("background", "black");
$("body, *").css("color", "fuchsia");
$(".bicons48 div.block-icon").css("height","0px");
}
});


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   