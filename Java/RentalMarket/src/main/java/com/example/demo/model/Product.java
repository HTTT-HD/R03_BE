package com.example.demo.model;


import org.springframework.boot.autoconfigure.domain.EntityScan;
import org.springframework.data.annotation.Id;

import org.springframework.data.mongodb.core.mapping.Document;


@EntityScan
@Document(collection = "sanpham")
public class Product {

	
	private@Id String _id;
	
	private String tensanpham;
	private String soluong;
	private String dongia;
	private String loaisanpham;
	private String cuahang;
	private String img;
	public Product() {}
	public Product(String name,String soluong, String dongia, String loaisanpham,String cuahang) {
		   this.tensanpham=name;
		   this.soluong = soluong;
		   this.dongia=dongia;
		   this.loaisanpham=loaisanpham;
		   this.cuahang=cuahang;
	   }

	public String getId() {
		return _id;
	}

	public void setId(String masanpham) {
		this._id = masanpham;
	}

	public String getCuahang() {
		return cuahang;
	}

	public void setCuahang(String cuahang) {
		this.cuahang = cuahang;
	}

	public String getTensanpham() {
		return tensanpham;
	}

	public void setTensanpham(String tensanpham) {
		this.tensanpham = tensanpham;
	}

	public String getSoluong() {
		return soluong;
	}

	public void setSoluong(String soluong) {
		this.soluong = soluong;
	}

	public String getDongia() {
		return dongia;
	}

	public void setDongia(String dongia) {
		this.dongia = dongia;
	}

	public String getLoaisanpham() {
		return loaisanpham;
	}

	public void setLoaisanpham(String loaisanpham) {
		this.loaisanpham = loaisanpham;
	}
	public String getImg() {
		return img;
	}
	public void setImg(String img) {
		this.img = img;
	}
	@Override
	public String toString() {
		return "Product [_id=" + _id + ", tensanpham=" + tensanpham + ", soluong=" + soluong + ", dongia=" + dongia
				+ ", loaisanpham=" + loaisanpham + ", cuahang=" + cuahang + ", img=" + img + "]";
	}
	
	
}
