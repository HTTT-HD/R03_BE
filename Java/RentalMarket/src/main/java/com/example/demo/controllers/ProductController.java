package com.example.demo.controllers;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.Random;

import org.bson.types.ObjectId;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.example.demo.exception.ResourceNotFoundException;
import com.example.demo.model.Product;
import com.example.demo.repository.ProductRepository;

@CrossOrigin(origins="*")
@RestController
@RequestMapping("/product")
public class ProductController {

	@Autowired
	private ProductRepository productRepository;
	@Autowired
    
	
	@GetMapping("")
	public List<Product> getAll(){
		return productRepository.findAll();
	}
	@GetMapping("/{id}")
	public Optional<Product> getProductById(@PathVariable(value = "id") String Id) {
		return productRepository.findById(Id);
	}
	@PostMapping("/add")
	public Product createEmployee(@Validated @RequestBody Product pro) {
  
        return productRepository.save(pro);
	}
	@PutMapping("/update/{id}")
	public ResponseEntity<String> updatePerson(@PathVariable(value = "id") String id, @Validated @RequestBody Product productDetails) {

		Optional<Product> temp = productRepository.findById(id);
		Product product=temp.get();
		System.out.println(product);
		product.setCuahang(productDetails.getCuahang());
		product.setTensanpham(productDetails.getTensanpham());
		product.setSoluong(productDetails.getSoluong());
		product.setDongia(productDetails.getDongia());
		product.setLoaisanpham(productDetails.getLoaisanpham());
		productRepository.save(product);
		
		return ResponseEntity.ok()
				.body("True");
	}
	@DeleteMapping("/del/{id}")
	public ResponseEntity<String> deletePerson(@PathVariable(value = "id") String id) {

		Optional<Product> temp = productRepository.findById(id);
		Product product=temp.get();
		productRepository.delete(product);
		return ResponseEntity.ok().body("True");
	}
}
