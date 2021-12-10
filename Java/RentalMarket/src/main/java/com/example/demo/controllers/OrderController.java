package com.example.demo.controllers;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.example.demo.model.Order;
import com.example.demo.repository.OrderRepository;

@CrossOrigin(origins="*")
@RestController
@RequestMapping("/order")
public class OrderController {
	@Autowired
	private OrderRepository orderRepository;
	@GetMapping("")
	public List<Order> getAll(){
		return orderRepository.findAll();
	}
}
