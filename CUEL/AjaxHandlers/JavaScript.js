/// <reference path="../scripts/jquery-3.3.1.js" />
/// <reference path="../scripts/knockout-3.4.2.js" />
var vm = new viewModel();
function viewModel() {
    var self = this;
    self.posts = ko.observable([]);
};

//$(window).load(function () {
ko.applyBindings(vm);
getPosts();
//});
//$('document').ready(function () {
//    alert('hi');
//});
function getPosts() {
    $.ajax({
        type: "POST",
        url: "/AjaxHandlers/CallMe.ashx",
        data: {
            "action": "GetPosts"
        },
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.Result != null) {
                for (var i in data.Result.$values) {
                    var p = data.Result.$values[i];
                    var pm = new post();
                    pm.postID(p.PostID);
                    pm.title(p.Title);
                    pm.description(p.Description);
                    pm.file(p.File);
                    pm.fileType(p.FileType);
                    pm.added(p.Added);
                    pm.appUserID(p.AppUserID);
                    pm.postComments(p.PostComments);
                    pm.appUser().appUserID(p.AppUser.AppUserID);
                    pm.appUser().appUserID(p.AppUser.FullName);
                    vm.posts().push(pm);
                }
            }
            else {
         //       alert(data.Status + ": " + data.Message);
            }
        },
        error: function (msg) {
        //    alert('Error:' + msg.d);
        }
    });
}
function post() {
    this.postID = ko.observable(0);
    this.title = ko.observable('');
    this.description = ko.observable('');
    this.file = ko.observable([]);
    this.fileType = ko.observable('');
    this.added = ko.observable('');
    this.appUserID = ko.observable(0);
    this.appUser = ko.observable(new appUser());
    this.postComments = ko.observable([]);
}
function appUser() {
    this.appUserID = ko.observable('');
    this.fullName = ko.observable('');
}
function postcommnet() {
    this.postCommentID = ko.observable(0);
    this.commentBody = ko.observable('');
    this.dateTime = ko.observable('');
    this.appUserID = ko.observable(0);
}