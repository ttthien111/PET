
$(function () {
    $(".billDetail").click(function () {
        var billId = $(this).attr("data-id");
        $.ajax({
            url: "/Admin/Bills/BillDetail",
            type: "GET",
            data: { billId },
            success: function (res) {
                console.log(res);
                var formatter = new Intl.NumberFormat('vi-VN', {
                    style: 'currency',
                    currency: 'VND',
                });

                // payment method
                var paymentMethod = ``;
                if (res.paymentMethodName == "PayPal") {
                    paymentMethod = `<i class="fa fa-paypal"></i>&nbsp; ` + res.paymentMethodName;
                } else {
                    paymentMethod = res.paymentMethodName;
                }

                // delivery state
                var theme = null;
                var status = null;

                if (res.isCancel) {
                    theme = "danger";
                    status = "Đã hủy";
                } else {
                    if (res.isApprove) {
                        theme = "info";
                        status = "Đã duyệt";
                    } else {
                        theme = "warning";
                        status = "Đang chờ xử lý";
                    }
                }

                // get bill detail
                var billDetail = "";
                var total = 0;
                for (var i = 0; i < res.billDetail.length; i++) {
                    total += res.billDetail[i].amount * res.billDetail[i].price;
                    billDetail +=
                        `<tr>
                            <td class="center">` + (i + 1) + `</td>
                            <td class="left strong">` + res.billDetail[i].productName + `</td>
                            <td class="left"><img style="width:50px" src="/images/products/`+ res.billDetail[i].image + `" alt="product image" /></td>
                            <td class="right">` + formatter.format(res.billDetail[i].price) + `</td >
                            <td class="center">` + res.billDetail[i].amount + `</td>
                            <td class="right">` + (formatter.format(res.billDetail[i].amount * res.billDetail[i].price)) + `</td>
                        </tr>`
                }

                // get footer
                var footer = null;
                if (!res.isCancel) {
                    if (!res.isApprove) {
                        footer = `
                                    <button type="button" class="btn btn-success" data-id="`+ res.billId + `" id="approveBill">Chốt đơn và đóng</button>
                                    <button type="button" class="btn btn-warning" data-dismiss="modal">Đóng</button>`
                    } else {
                        footer = `<button type="button" class="btn btn-warning" data-dismiss="modal">Đóng</button>`
                    }
                } else {
                    footer = `<button type="button" class="btn btn-warning" data-dismiss="modal">Đóng</button>`
                }

                var content = `<div class="modal-header">
                                    <h4 class="modal-title">Chi tiết hóa đơn <span id="billIdModal"></span></h4>
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                                        <div id="billDetailContent">
                                                        <div class="p-4">
                                                            <a class="pt-2 d-inline-block" asp-controller="Home" asp-action="Index"><span class="flaticon-pawprint-1 mr-2"></span><b>PETSHOP</b></a>
                                                            <div class="float-right">
                                                                <h3 class="mb-0 text-danger text">Đơn hàng #`+ res.billCode + `</h3>
                                                                Ngày: ` + res.dateOfPurchase + `
                                                            </div>
                                                        </div>
                                                        <div class="">
                                                            <div class="row mb-4">
                                                                <div class="col-sm-6">
                                                                    <h5 class="mb-3">Thông tin vận chuyển:</h5>
                                                                    <div>Số điện thoại: <span class="text text-primary">` + res.delivery.deliveryProductPhoneNumber + `</span></div>
                                                                    <div>Địa chỉ: <span class="text text-primary">` + res.delivery.deliveryProductAddress + `</span></div>
                                                                    <div>Ghi chú: <span class="text text-primary">` + res.delivery.deliveryProductNote + `</span></div>
                                                                    <div>
                                                                        Phương thức thanh toán:
                                                                        <span class="text text-primary">` +
                                                                            paymentMethod + `
                                                                        </span>
                                                                    </div>
                                                                    <div>
                                                                        Trạng thái vận chuyển:
                                                                        <span class="text text-success">` + res.deliveryStateName + `</span>
                                                                    </div>
                                                                    <div>
                                                                        Trạng thái đơn hàng:
                                                                        <span class="text text-` + theme + `">` + status + `</span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="table-responsive-sm">
                                                                <table class="table table-striped">
                                                                    <thead>
                                                                        <tr>
                                                                            <th class="center">#</th>
                                                                            <th>Tên hàng hóa</th>
                                                                            <th>Mô tả</th>
                                                                            <th class="right">Giá</th>
                                                                            <th class="center">Số lượng</th>
                                                                            <th class="right">Tổng cộng</th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>`
                                                                        + billDetail + `
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-lg-4 col-sm-4 ml-auto">
                                                                    <table class="table table-clear">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td class="left">
                                                                                    <strong class="text-dark">Tổng đơn hàng</strong>
                                                                                </td>
                                                                                <td class="right"> <b>` + formatter.format(total) + `</b></td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                                <div class="col-lg-4 col-sm-4 ml-auto">
                                                                    <table class="table table-clear">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td class="left">
                                                                                    <strong class="text-dark">Phí vận chuyển</strong>
                                                                                </td>
                                                                                <td class="right"> <b>` + formatter.format(res.totalPrice - total) + `</b></td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                                <div class="col-lg-4 col-sm-4 ml-auto">
                                                                    <table class="table table-clear">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td class="left">
                                                                                    <strong class="text-dark">Tổng tiền</strong>
                                                                                </td>
                                                                                <td class="right"> <b>` + formatter.format(res.totalPrice) + `</b></td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="card-footer bg-white">
                                                            <p class="mb-0">280 An Dương Vương, Phường 4, Quận 5, TPHCM</p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="modal-footer">
                                                ` + footer + `
                                            </div>`

                document.getElementById("billDetailContent").innerHTML = content;
            },
            error: function (err) {
                alert("What's wrong!");
                console.log(err);
            }
        });
    });
});


$(document).on('click', '#approveBill', function () {
    var billId = $(this).attr("data-id");
    let isApprove = confirm("Bạn có muốn chốt đơn hàng #" + billId + " này không ? Vui lòng snapshot lại hóa đơn này tại thư mục D:/OrderImage với tên là mã đơn hàng bên trên. Nếu không sẽ không thể chốt đơn");
    if (isApprove) {
        $.ajax({
            url: "/Admin/Bills/ApproveBill",
            type: "POST",
            data: { billId },
            success: function (data) {
                window.location.reload();
            },
            error: function (err) {
                alert("Không thể chốt đơn !");
            }
        });
    }
});