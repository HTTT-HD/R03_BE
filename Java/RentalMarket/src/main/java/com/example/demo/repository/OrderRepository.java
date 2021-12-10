package com.example.demo.repository;


import org.bson.types.ObjectId;
import org.springframework.data.mongodb.repository.MongoRepository;
import org.springframework.stereotype.Repository;

import com.example.demo.model.Order;


public interface OrderRepository extends MongoRepository<Order,String> {

}
