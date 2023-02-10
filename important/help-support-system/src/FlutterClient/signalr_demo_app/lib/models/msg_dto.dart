class MsgDto {
  int Id;
  String From;
  String To;
  String Content;
  DateTime? MsgTime;

  MsgDto(
      {required this.Id,
      required this.From,
      required this.To,
      required this.Content,
      required this.MsgTime});

  MsgDto.fromJson(Map<String, dynamic> json)
      : Id = json['id'],
        From = json['from'],
        To = json['to'],
        Content = json['content'],
        MsgTime = DateTime.tryParse(json['msg_time'] + "Z");

  Map<String, dynamic> toMap(String chatId) {
    return {
      'id': Id,
      'chatId': chatId,
      'sender': From,
      'target': To,
      'content': Content,
      'msgTime': MsgTime!.toUtc().toString(),
    };
  }
}
