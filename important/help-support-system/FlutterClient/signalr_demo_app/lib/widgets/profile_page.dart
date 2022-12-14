import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/controllers/profileController.dart';

class ProfilePage extends GetWidget<ProfileController> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(32),
        child: Row(
          children: [
            Expanded(
                child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                /*2*/
                Container(
                  padding: const EdgeInsets.only(bottom: 8),
                  child: Text(
                    controller.Profile.UserName,
                    style: TextStyle(
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                ),
                Text(
                  controller.Profile.Email,
                  style: TextStyle(
                    color: Colors.grey[500],
                  ),
                ),
                Text(
                  controller.Profile.UserId,
                  style: TextStyle(
                    color: Colors.grey[500],
                  ),
                ),
              ],
            )),
            /*3*/
            Icon(
              Icons.star,
              color: Colors.amber,
            ),
            const Text('99'),
          ],
        ),
      ),
    );
  }
}
