package com.example.demo.model;


import java.util.Arrays;

import org.springframework.boot.autoconfigure.domain.EntityScan;
import org.springframework.data.annotation.Id;

import org.springframework.data.mongodb.core.mapping.Document;


@EntityScan
@Document(collection = "donhang")
public class Order {

	private @Id String madonhang;
	private String magiamgia;
	private String diachigiao;
	private String nguoimua;
	private String nguoigiaohang;
	private String ngaydat;
	private String ngaygiao;
	private String Hinhthucthanhtoan;
	private String Tinhtrang;
	private Product sanpham[];
	
	public Product[] getSanpham() {
		return sanpham;
	}
	public void setSanpham(Product[] sanpham) {
		this.sanpham = sanpham;
	}
	public String getMagiamgia() {
		return magiamgia;
	}
	public void setMagiamgia(String magiamgia) {
		this.magiamgia = magiamgia;
	}
	public String getDiachigiao() {
		return diachigiao;
	}
	public void setDiachigiao(String diachigiao) {
		this.diachigiao = diachigiao;
	}
	public String getNguoimua() {
		return nguoimua;
	}
	public void setNguoimua(String nguoimua) {
		this.nguoimua = nguoimua;
	}
	public String getNguoigiaohang() {
		return nguoigiaohang;
	}
	public void setNguoigiaohang(String nguoigiaohang) {
		this.nguoigiaohang = nguoigiaohang;
	}
	public String getNgaydat() {
		return ngaydat;
	}
	public void setNgaydat(String ngaydat) {
		this.ngaydat = ngaydat;
	}
	public String getNgaygiao() {
		return ngaygiao;
	}
	public void setNgaygiao(String ngaygiao) {
		this.ngaygiao = ngaygiao;
	}
	public String getHinhthucthanhtoan() {
		return Hinhthucthanhtoan;
	}
	public void setHinhthucthanhtoan(String hinhthucthanhtoan) {
		Hinhthucthanhtoan = hinhthucthanhtoan;
	}
	public String getTinhtrang() {
		return Tinhtrang;
	}
	public void setTinhtrang(String tinhtrang) {
		Tinhtrang = tinhtrang;
	}

	@Override
	public String toString() {
		return "Order [madonhang=" + madonhang + ", magiamgia=" + magiamgia + ", diachigiao=" + diachigiao
				+ ", nguoimua=" + nguoimua + ", nguoigiaohang=" + nguoigiaohang + ", ngaydat=" + ngaydat + ", ngaygiao="
				+ ngaygiao + ", Hinhthucthanhtoan=" + Hinhthucthanhtoan + ", Tinhtrang=" + Tinhtrang + ", sanpham="
				+ Arrays.toString(sanpham) + "]";
	}
	
	
	
	
}
