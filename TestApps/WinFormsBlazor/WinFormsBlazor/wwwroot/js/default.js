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


//�޸����
function comma(str) {
	str = String(str);
	return str.replace(/(\d)(?=(?:\d{3})+(?!\d))/g, '$1,');
}

//�޸�Ǯ��
function uncomma(str) {
	str = String(str);
	return str.replace(/[^\d]+/g, '');
}