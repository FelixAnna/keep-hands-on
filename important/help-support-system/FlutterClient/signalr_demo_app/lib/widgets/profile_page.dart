import 'dart:convert';

import 'package:flutter/material.dart';

import '../models/login_response.dart';
import '../utils/localstorage_service.dart';

class ProfilePage extends StatefulWidget {
  @override
  State<ProfilePage> createState() => _ProfilePageState();
}

class _ProfilePageState extends State<ProfilePage> {
  late User profile;

  @override
  void initState() {
    profile = new User(UserId: "", UserName: "", Email: "");
    _init();
    super.initState();
  }

  _init() async {
    var profileText =
        await LocalStorageService.get(LocalStorageService.PROFILE);
    setState(() {
      profile = User.fromJson(jsonDecode(profileText));
    });
  }

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
                    profile.UserName,
                    style: TextStyle(
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                ),
                Text(
                  profile.Email,
                  style: TextStyle(
                    color: Colors.grey[500],
                  ),
                ),
                Text(
                  profile.UserId,
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
