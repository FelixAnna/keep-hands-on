import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:signalr_demo_app/models/contact.dart';
import '../models/login_response.dart';
import '../services/user_service.dart';
import '../utils/localstorage_service.dart';
import 'conversation_item.dart';
import '../models/chat_member.dart';

class ChatContainerPage extends StatefulWidget {
  final UserService userService;
  const ChatContainerPage(
    this.userService,
  );

  @override
  _ChatContainerPageState createState() => _ChatContainerPageState();
}

class _ChatContainerPageState extends State<ChatContainerPage>
    with AutomaticKeepAliveClientMixin<ChatContainerPage> {
  List<ChatMember> chatGroups = [];
  List<ChatMember> chatUsers = [];
  late final User profile;

  @override
  bool get wantKeepAlive => false;

  @override
  initState() {
    super.initState();
    _init();
    loadProfile();
  }

  _init() async {
    final contact = await widget.userService.getCurrentContacts();

    for (GroupInfo item in contact.Groups) {
      setState(() {
        chatGroups = [
          ...chatGroups,
          new ChatMember(
              talkingTo: item.GroupId,
              name: item.Name,
              type: "group",
              messageText: "Hello Everyone",
              imageURL:
                  "https://thumbs.dreamstime.com/z/little-cats-20284533.jpg",
              time: "Now")
        ];
      });
    }

    for (MemberInfo item in contact.Friends) {
      setState(() {
        chatUsers = [
          ...chatUsers,
          new ChatMember(
              talkingTo: item.UserId,
              name: item.UserName,
              type: "user",
              messageText: "Good morning",
              imageURL:
                  "https://cdn.pixabay.com/photo/2015/11/16/14/43/cat-1045782__340.jpg",
              time: "Now")
        ];
      });
    }
  }

  loadProfile() async {
    var profileText =
        await LocalStorageService.get(LocalStorageService.PROFILE);

    setState(() {
      profile = User.fromJson(jsonDecode(profileText));
    });
  }

  @override
  Widget build(BuildContext context) {
    //Notice the super-call here.
    super.build(context);
    return Scaffold(
      body: SingleChildScrollView(
        physics: BouncingScrollPhysics(),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            SafeArea(
              child: Padding(
                padding: EdgeInsets.only(left: 16, right: 16, top: 10),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: <Widget>[
                    Text(
                      "Conversations",
                      style:
                          TextStyle(fontSize: 32, fontWeight: FontWeight.bold),
                    ),
                    Container(
                      padding:
                          EdgeInsets.only(left: 8, right: 8, top: 2, bottom: 2),
                      height: 30,
                      decoration: BoxDecoration(
                        borderRadius: BorderRadius.circular(30),
                        color: Colors.pink[50],
                      ),
                      child: Row(
                        children: <Widget>[
                          Icon(
                            Icons.add,
                            color: Colors.pink,
                            size: 20,
                          ),
                          SizedBox(
                            width: 2,
                          ),
                          Text(
                            "Add New",
                            style: TextStyle(
                                fontSize: 14, fontWeight: FontWeight.bold),
                          ),
                        ],
                      ),
                    )
                  ],
                ),
              ),
            ),
            ListView.builder(
              itemCount: chatGroups.length,
              shrinkWrap: true,
              padding: EdgeInsets.only(top: 16),
              physics: NeverScrollableScrollPhysics(),
              itemBuilder: (context, index) {
                return ConversationItem(
                  profile: profile,
                  member: chatGroups[index],
                  isMessageRead: (index == 0 || index == 3) ? true : false,
                );
              },
            ),
            ListView.builder(
              itemCount: chatUsers.length,
              shrinkWrap: true,
              padding: EdgeInsets.only(top: 16),
              physics: NeverScrollableScrollPhysics(),
              itemBuilder: (context, index) {
                return ConversationItem(
                  profile: profile,
                  member: chatUsers[index],
                  isMessageRead: (index == 0 || index == 3) ? true : false,
                );
              },
            ),
          ],
        ),
      ),
    );
  }
}
