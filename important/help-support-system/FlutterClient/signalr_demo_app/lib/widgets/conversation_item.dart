import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/models/chat_member.dart';

import '../models/login_response.dart';
import '../utils/localstorage_service.dart';
import 'chat_details_page.dart';
import 'group_chat_details_page.dart';

class ConversationItem extends StatefulWidget {
  final ChatMember member;
  final bool isMessageRead;
  ConversationItem({
    required this.member,
    required this.isMessageRead,
  });

  @override
  _ConversationItemState createState() => _ConversationItemState();
}

class _ConversationItemState extends State<ConversationItem> {
  late final User profile;
  loadProfile() async {
    var profileText =
        await LocalStorageService.get(LocalStorageService.PROFILE);

    profile = User.fromJson(jsonDecode(profileText));
  }

  @override
  void initState() {
    super.initState();
    loadProfile();
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {
        if (widget.member.type == "user") {
          Get.to(() => ChatDetailsPage(
                profile: profile,
                chatId: widget.member.talkingTo,
                name: widget.member.name,
              ));
        } else {
          Get.to(() => GroupChatDetailsPage(
                profile: profile,
                chatId: widget.member.talkingTo,
                name: widget.member.name,
              ));
        }
      },
      child: Container(
        padding: EdgeInsets.only(left: 16, right: 16, top: 10, bottom: 10),
        child: Row(
          children: <Widget>[
            Expanded(
              child: Row(
                children: <Widget>[
                  CircleAvatar(
                    backgroundImage: NetworkImage(widget.member.imageURL),
                    maxRadius: 30,
                  ),
                  SizedBox(
                    width: 16,
                  ),
                  Expanded(
                    child: Container(
                      color: Colors.transparent,
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: <Widget>[
                          Text(
                            widget.member.name,
                            style: TextStyle(fontSize: 16),
                          ),
                          SizedBox(
                            height: 6,
                          ),
                          Text(
                            widget.member.messageText,
                            style: TextStyle(
                                fontSize: 13,
                                color: Colors.grey.shade600,
                                fontWeight: widget.isMessageRead
                                    ? FontWeight.bold
                                    : FontWeight.normal),
                          ),
                        ],
                      ),
                    ),
                  ),
                ],
              ),
            ),
            Text(
              widget.member.time,
              style: TextStyle(
                  fontSize: 12,
                  fontWeight: widget.isMessageRead
                      ? FontWeight.bold
                      : FontWeight.normal),
            ),
          ],
        ),
      ),
    );
  }
}
