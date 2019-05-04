
  $('.menu-item').on('mouseover',function(){
    let t = $(this).children()[1];
    if(!$(t).css('display','block')){
            $(this).children()[1].show();
          }
          console.log()
  })


    $('#bottom-menu').on('mouseout',function(){
      $('.options').hide();
    })
