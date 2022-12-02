class Group {
  String GroupId;
  String Name;
  Group({required this.GroupId, required this.Name});

  Group.fromJson(Map<String, dynamic> json)
      : GroupId = json['groupId'].toString(),
        Name = json['name'];

  Map<String, dynamic> toJson() => {
        'groupId': this.GroupId,
        'name': this.Name,
      };
}
