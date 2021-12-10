package com.example.demo.controllers;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.example.demo.model.Seller;
import com.example.demo.repository.ProductRepository;
import com.example.demo.repository.SellerRepository;

@CrossOrigin(origins="*")
@RestController
@RequestMapping("/seller")
public class SellerController {
	@Autowired
	private SellerRepository sellerRepository;
	@GetMapping("")
	public List<Seller> getAll(){
		return sellerRepository.findAll();
	}
}
