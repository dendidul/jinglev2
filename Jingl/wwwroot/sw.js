self.addEventListener('push', function (event) {
    if (!(self.Notification && self.Notification.permission === 'granted')) {
        return;
    }

    var data = {};
    if (event.data) {
        data = event.data.json();
    }

    console.log('Notification Received:');
    console.log(data);

    var title = data.title;
    var message = data.message;
    var icon = "template/img/bgr_1.png";
    
    event.waitUntil(self.registration.showNotification(title, {
        body: message,
        icon: icon,
        badge: icon,
        //data: {
        //    url: "https://jingl.azurewebsites.net/"
        //}
    }));
});

self.addEventListener('notificationclick', function (event) {
    var data = {};
    if (event.data) {
        data = event.data.json();
    }
    clients.openWindow("https://www.fameoapp.com/Account/Profile/");
    //clients.openWindow("https://localhost:44374/Activity/Activity/");
  
    //clients.openWindow(data.url);

    event.notification.close();
});
