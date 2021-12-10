package com.example.demo.model;
import org.springframework.boot.autoconfigure.domain.EntityScan;
import org.springframework.data.annotation.Id;

import org.springframework.data.mongodb.core.mapping.Document;
@EntityScan
@Document(collection = "nguoiban")
public class Seller {
	
	private @Id String manguoiban;
	private String tennguoiban;
	private String sdt;
	private String diachi;
	private String cmnd;
	private String ngaysinh;
	private String gioitinh;
	private String img;
	private String matkhau;
	private String tendangnhap;
	
	
	public Seller() {}

	public String getTennguoiban() {
		return tennguoiban;
	}

	public void setTennguoiban(String tennguoiban) {
		this.tennguoiban = tennguoiban;
	}

	public String getSdt() {
		return sdt;
	}

	public void setSdt(String sdt) {
		this.sdt = sdt;
	}

	public String getDiachi() {
		return diachi;
	}

	public void setDiachi(String diachi) {
		this.diachi = diachi;
	}

	public String getCmnd() {
		return cmnd;
	}

	public void setCmnd(String cmnd) {
		this.cmnd = cmnd;
	}

	public String getNgaysinh() {
		return ngaysinh;
	}

	public void setNgaysinh(String ngaysinh) {
		this.ngaysinh = ngaysinh;
	}

	public String getGioitinh() {
		return gioitinh;
	}

	public void setGioitinh(String gioitinh) {
		this.gioitinh = gioitinh;
	}

	public String getImg() {
		return img;
	}

	public void setImg(String img) {
		this.img = img;
	}

	public String getMatkhau() {
		return matkhau;
	}

	public void setMatkhau(String matkhau) {
		this.matkhau = matkhau;
	}

	public String getTendangnhap() {
		return tendangnhap;
	}

	public void setTendangnhap(String tendangnhap) {
		this.tendangnhap = tendangnhap;
	}

	@Override
	public String toString() {
		return "Seller [manguoiban=" + manguoiban + ", tennguoiban=" + tennguoiban + ", sdt=" + sdt + ", diachi="
				+ diachi + ", cmnd=" + cmnd + ", ngaysinh=" + ngaysinh + ", gioitinh=" + gioitinh + ", img=" + img
				+ ", matkhau=" + matkhau + ", tendangnhap=" + tendangnhap + "]";
	}

};
	
