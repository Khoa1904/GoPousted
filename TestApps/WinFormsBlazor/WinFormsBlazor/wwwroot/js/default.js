window.CalPrice = (num, gubun, price) => {
	let cnt = $("#cnt" + num).val();
	if (gubun == "plus") {
		cnt++;
	} else {
		if (cnt > 1) {
			cnt--;
		}
	}

	const tPrice = price * cnt;

	$("#cnt" + num).val(cnt);
	$("#priceTxt" + num).html(comma(tPrice));
};


//ÄÞ¸¶Âï±â
function comma(str) {
	str = String(str);
	return str.replace(/(\d)(?=(?:\d{3})+(?!\d))/g, '$1,');
}

//ÄÞ¸¶Ç®±â
function uncomma(str) {
	str = String(str);
	return str.replace(/[^\d]+/g, '');
}