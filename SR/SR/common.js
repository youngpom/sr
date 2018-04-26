function alertMessage(message, redirectpage){
    alert(message);
    document.location.href(redirectpage);
}

function SearchDept( DeptseqQuery, DeptseqNmQuery, DevempNm ) {
    window.open("./searchDept.aspx?DeptseqQuery=" + DeptseqQuery + "&DeptseqNmQuery=" + DeptseqNmQuery + "&txtSearch=" + DevempNm, "", "toolbar=false, menubar=false, scrollbars=yes, resizable=yes");
    //DeptseqQuery   => 부서코드가 샛팅될 Item을 정의,'form1.txtDeptseq.value'
    //DeptseqNmQuery => 부서명이  샛팅될 Item을 정의,'form1.txtDeptseqNm.value'
}

function SearchDeptCheckOne( DeptseqQuery, DeptseqNmQuery, DevempNm ) {
    window.open("./searchDept.aspx?OnlyOne=Y&DeptseqQuery=" + DeptseqQuery + "&DeptseqNmQuery=" + DeptseqNmQuery + "&txtSearch=" + DevempNm, "", "toolbar=false, menubar=false, scrollbars=yes, resizable=yes");
    //DeptseqQuery   => 부서코드가 샛팅될 Item을 정의,'form1.txtDeptseq.value'
    //DeptseqNmQuery => 부서명이  샛팅될 Item을 정의,'form1.txtDeptseqNm.value'
}

function SearchReqemp(ReqempseqQuery, ReqempNmQuery) {
    window.open("./searchReqeuestBy.aspx?ReqempseqQuery=" + ReqempseqQuery + "&ReqempNmQuery=" + ReqempNmQuery, "", "toolbar=false, menubar=false, scrollbars=yes, resizable=yes");
    //ReqempseqQuery => 요청자코드가 샛팅될 Item을 정의,'form1.txtReqempseq.value'
    //ReqempNmQuery  => 요청자명이  샛팅될 Item을 정의,'form1.txtReqempNm.value'
}

function SearchReqempCheckOne(ReqempseqQuery, ReqempNmQuery, ReqempNm) {
    window.open("./searchReqeuestBy.aspx?OnlyOne=Y&ReqempseqQuery=" + ReqempseqQuery + "&ReqempNmQuery=" + ReqempNmQuery + "&txtSearch=" + ReqempNm, "", "toolbar=false, menubar=false, scrollbars=yes, resizable=yes");
    //ReqempseqQuery => 요청자코드가 샛팅될 Item을 정의,'form1.txtReqempseq.value'
    //ReqempNmQuery  => 요청자명이  샛팅될 Item을 정의,'form1.txtReqempNm.value'
}


function SearchDevemp(DevempIdQuery, DevempNmQuery) {
    window.open("./searchReceiptBy.aspx?DevempIdQuery=" + DevempIdQuery + "&DevempNmQuery=" + DevempNmQuery, "", "toolbar=false, menubar=false, scrollbars=yes, resizable=yes");
    //DevempIdQuery   => 개발자ID가 샛팅될 Item을 정의,'form1.txtDevempId.value'
    //DevempNmQuery   => 개발자명이  샛팅될 Item을 정의,'form1.txtDevempNm.value'
}

function SearchDevempCheckOne(DevempIdQuery, DevempNmQuery, DevempNm) {
    window.open("./searchReceiptBy.aspx?OnlyOne=Y&DevempIdQuery=" + DevempIdQuery + "&DevempNmQuery=" + DevempNmQuery + "&txtSearch=" + DevempNm, "", "toolbar=false, menubar=false, scrollbars=yes, resizable=yes");
    //DevempIdQuery   => 개발자ID가 샛팅될 Item을 정의,'form1.txtDevempId.value'
    //DevempNmQuery   => 개발자명이  샛팅될 Item을 정의,'form1.txtDevempNm.value'
}

