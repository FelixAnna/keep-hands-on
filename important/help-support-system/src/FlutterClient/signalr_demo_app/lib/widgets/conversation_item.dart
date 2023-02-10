import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/models/chat_member.dart';

import '../controllers/chatDetailController.dart';
import '../controllers/groupChatDetailController.dart';
import '../models/user.dart';
import 'chat_details_page.dart';
import 'group_chat_details_page.dart';

class ConversationItem extends StatelessWidget {
  final ChatMember member;
  final bool isMessageRead;
  final User profile;
  ConversationItem({
    required this.member,
    required this.isMessageRead,
    required this.profile,
  });

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () async {
        if (this.member.type == "user") {
          var isRegistered = Get.isRegistered<ChatDetailController>(
              tag: this.member.talkingTo);
          if (!isRegistered) {
            var chatController = ChatDetailController(
              chatId: this.member.talkingTo,
              name: this.member.name,
            );
            await chatController.initial();
            chatController.subscribe(Get.find());
            Get.put(chatController, tag: this.member.talkingTo);
          }

          Get.to(() => ChatDetailsPage(
                chatId: this.member.talkingTo,
                name: this.member.name,
                avatarUrl: this.member.imageURL,
              ));
        } else {
          var isRegistered = Get.isRegistered<GroupChatDetailController>(
              tag: this.member.talkingTo);
          if (!isRegistered) {
            var chatController = GroupChatDetailController(
              chatId: this.member.talkingTo,
              name: this.member.name,
            );
            await chatController.initial();
            chatController.subscribe(Get.find());
            Get.put(chatController, tag: this.member.talkingTo);
          }

          Get.to(() => GroupChatDetailsPage(
                chatId: this.member.talkingTo,
                name: this.member.name,
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
                    backgroundImage: NetworkImage(this.member.imageURL),
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
                            this.member.name,
                            style: TextStyle(fontSize: 16),
                          ),
                          SizedBox(
                            height: 6,
                          ),
                          Text(
                            this.member.messageText,
                            style: TextStyle(
                                fontSize: 13,
                                color: Colors.grey.shade600,
                                fontWeight: this.isMessageRead
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
              this.member.time,
              style: TextStyle(
                  fontSize: 12,
                  fontWeight:
                      this.isMessageRead ? FontWeight.bold : FontWeight.normal),
            ),
            VerticalDivider(),
            Text(
              this.member.unread.toString(),
              style: TextStyle(
                  fontSize: 14,
                  backgroundColor: Colors.lightBlue,
                  fontWeight:
                      this.isMessageRead ? FontWeight.bold : FontWeight.normal),
            ),
          ],
        ),
      ),
    );
  }
}
