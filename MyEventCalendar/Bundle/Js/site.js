$(document).ready(function () {
	var owl = $('.owl-carousel');
	owl.owlCarousel({
		items: 1,
		loop: true,
		// margin:0,
		autoplay: true,
		autoplayTimeout: 3000,
		autoplayHoverPause: true
	});

	new WOW().init();

	//counter up
	$('.counter').counterUp({
		delay: 10,
		time: 2000
	});

	// ajax load Detail Country
	function loadDetailCountry(ItemID) {
		$.ajax({
			type: "GET",
			url: "/api/sitecore/Country/DetailCountry",
			contentType: "application/json",
			data: { ID: ItemID },
			success: function (response) {
				if (response.length > 0) {
					$('#detail').html(response);
				}

			},
			error: function () {
				console.log("Error with Ajax !!!");
			}
		});

	}
	/////
	var coll = document.getElementsByClassName("click-real");
	var i;

	for (i = 0; i < coll.length; i++) {
		coll[i].addEventListener("click", function () {
			var parent = $(this).parent();
			var currentPostion = $(this).position();
			var nextPosition = $(this).next().position();
			var content = parent.find('#detail');
			var currentDisplay = content.is(":visible");
			var id = 1;
			var relDetailId = 0;
			var temp;
			if (nextPosition != undefined) {
				if (currentPostion.top === nextPosition.top) {
					currentPostion = $(this).next().position();
					nextPosition = $(this).next().next().position();
					if (nextPosition != undefined) {
						if (currentPostion.top != nextPosition.top) {
							id = $(this).next().attr('ref').split('_')[1];
							parent.find('div[ref^="data_' + id + '" ]').after($('#detail'));
						} else {
							id = $(this).next().next().attr('ref').split('_')[1];
							parent.find('div[ref^="data_' + id + '" ]').after($('#detail'));
						}
					} else {
						id = $(this).next().attr('ref').split('_')[1];
						parent.find('div[ref^="data_' + id + '" ]').after($('#detail'));
					}
				} else {
					id = $(this).attr('ref').split('_')[1];
					parent.find('div[ref^="data_' + id + '" ]').after($('#detail'));
				}
			} else {
				id = $(this).attr('ref').split('_')[1];
				parent.find('div[ref^="data_' + id + '" ]').after($('#detail'));
			}

			temp = $('#detail').attr('ref');
			$('#detail').attr('ref', 'detail_' + $(this).attr('ref').split('_')[1]);
			var content = parent.find('#detail');

			var currentDisplay = temp === $('#detail').attr('ref') ? content.is(":visible") : false;


			//load Data Detail
			loadDetailCountry($(this).attr('ref').split('_')[1]);

			//this.classList.toggle("active");            
			parent.removeClass('active');
			parent.find('div[id^="detail"]').hide();
			if (currentDisplay) {
				content.hide();
			} else {
				content.show();
			}
		});
	}

});







