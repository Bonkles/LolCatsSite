

//This function queries the lolCat database and gets the image URLs to display.
//Then it creates a list item (<LI>) for each image inside the existing UL with id 'slideshow'.

//The list item 
//<li><img src="images/s3.jpg" title="Slide 3" alt="Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor."/></li>

lolCatsLoaded = false;
/*
function assembleSlides() {

    //Prevent the repeated loading of lolCats from the database. We don't want to hammer 
    //the database needlessly. 
    if (lolCatsLoaded == false) {
        $.getJSON("api/lolCats/",
        function (data) {
            var i = 0;
            // On success, 'data' contains a list of lolCats.
            $.each(data, function (key, val) {
                var slideShowStr;
                // Add the list item.
                slideShowStr = '<img src="../../lolcat_inbox/' + val.ImgFilePath + '" alt="Kitteh!">';

                // Add a list item for the kitty inside the slideshow <ul>.
                //                if (i == 0) {
                //                    $('<li class="show"/>', { html: slideShowStr })
                //                .appendTo($('ul.slideshow'));
                //                }
                //                else {
                $('<li/>', { html: slideShowStr })
                .appendTo($('ul.slideshow'));
                //                }
                i++;
            }); //end 'each' statement

            //Find the first element that we just wrote, and add the 'show' class to it. 
            //This will cause the slideshow to pick it up and display it. 
            $('ul.slideshow li:first').addClass('show');
            lolCatsLoaded = true;
        });  //end 'function' statement
    } //end if lolCatsLoaded == false
}
*/
    function dataCallback () 
    {
        return function (data) {
            var i = 0;
            // On success, 'data' contains a list of lolCats.
            $.each(data, function (key, val) {
                var slideShowStr;
                // Add the list item.
                slideShowStr = '<img src="../../lolcat_inbox/' + val.ImgFilePath + '" alt="Kitteh!">';

                // Add a list item for the kitty inside the slideshow <ul>.
                //                if (i == 0) {
                //                    $('<li class="show"/>', { html: slideShowStr })
                //                .appendTo($('ul.slideshow'));
                //                }
                //                else {
                $('<li/>', { html: slideShowStr })
                .appendTo($('ul.slideshow'));
                //                }
                i++;
            }); //end 'each' statement

            //Find the first element that we just wrote, and add the 'show' class to it. 
            //This will cause the slideshow to pick it up and display it. 
            $('ul.slideshow li:first').addClass('show');
            lolCatsLoaded = true;
        };
    }  //end 'function' statement


function assembleSlides() {

    //Prevent the repeated loading of lolCats from the database. We don't want to hammer 
    //the database needlessly. 
    if (lolCatsLoaded == false) {
    $.getJSON("api/lolCats/", dataCallback())
    }//end if lolCatsLoaded == false
}

function slideShow(speed) {


	//append a LI item to the UL list for displaying caption
	$('ul.slideshow').append('<li id="slideshow-caption" class="caption"><div class="slideshow-caption-container"><h3></h3><p></p></div></li>');

	//Set the opacity of all images to 0
	$('ul.slideshow li').css({opacity: 0.0});
	
	//Get the first image and display it (set it to full opacity)
	$('ul.slideshow li:first').css({opacity: 1.0}).addClass('show');
	
	//Get the caption of the first image from REL attribute and display it
	$('#slideshow-caption h3').html($('ul.slideshow li.show').find('img').attr('title'));
	$('#slideshow-caption p').html($('ul.slideshow li.show').find('img').attr('alt'));
		
	//Display the caption
	$('#slideshow-caption').css({opacity: 0.7, bottom:0});
	
	//Call the gallery function to run the slideshow	
	var timer = setInterval('gallery()',speed);
	
	//pause the slideshow on mouse over
	$('ul.slideshow').hover(
		function () {
			clearInterval(timer);	
		}, 	
		function () {
			timer = setInterval('gallery()',speed);			
		}
	);
	
}

function gallery() {


	//if no IMGs have the show class, grab the first image
	var current = ($('ul.slideshow li.show')?  $('ul.slideshow li.show') : $('#ul.slideshow li:first'));
	
	//trying to avoid speed issue
	if(current.queue('fx').length == 0) {	
	
		//Get next image, if it reached the end of the slideshow, rotate it back to the first image
		var next = ((current.next().length) ? ((current.next().attr('id') == 'slideshow-caption')? $('ul.slideshow li:first') :current.next()) : $('ul.slideshow li:first'));
			
		//Get next image caption
		var title = next.find('img').attr('title');	
		var desc = next.find('img').attr('alt');	
	
		//Set the fade in effect for the next image, show class has higher z-index
		next.css({opacity: 0.0}).addClass('show').animate({opacity: 1.0}, 1000);
		
		//Hide the caption first, and then set and display the caption
			$('#slideshow-caption h3').html(title); 
			$('#slideshow-caption p').html(desc); 
	
		//Hide the current image
		current.animate({opacity: 0.0}, 1000).removeClass('show');

	}


}
