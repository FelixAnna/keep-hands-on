import 'package:signalr_demo_app/models/msg_dto.dart';

class ChatMessages {
  late String ChatId;
  late String LatestMsge;
  late int UnreadCount;
  late DateTime? Time;
  late List<MsgDto> Messages;

  ChatMessages(
      {required this.ChatId,
      required this.LatestMsge,
      required this.UnreadCount,
      required this.Time,
      required this.Messages});
}
