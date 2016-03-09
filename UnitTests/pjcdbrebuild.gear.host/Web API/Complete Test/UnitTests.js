    var uri = 'http://pjcdbrebuild.gear.host/api/';
    var successes = 0;
    var attempts = 0;
    
    function success() {
      successes = successes + 1;
      attempts = attempts + 1;
      $('#successes').html('' + successes);
      $('#attempts').html('' + attempts);
    }

    function failure() {
      attempts = attempts + 1;
      $('#attempts').html('' + attempts);
      $('#tally').css('color','red');
    }

    function complete() {
      $('#complete').html('Unit Tests Complete');
    }

    $(document).ready(function () {
      HttpRoutingTest();
      //AuthenticationAndValidation();
      //PasswordChange();
    });

<!-- %%%%%%%%% HTTP Routing %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% -->

    function HttpRoutingTest() {
      //Start
      HTTPgetAllHello();
      //post();
      //getID(id);
      //put();
      //getLang(lang);
      //finalHello();
    }

    function HTTPgetAllHello(){
      // Send an AJAX request
      $.getJSON(uri + "Hello")
      .done(function (data) {
        // On success, 'data' contains a list of products.
        $.each(data, function (key, item) {
          // Add a list item for the record.
          $('<li>', { text: formatHelloItem(item) }).appendTo($('#getAllResults'));
        });
        
        var originalNum = $('#getAllResults li').length;

        if (originalNum == 0){
          $('#getAllPF').html('Failure: No Results');
          $('#getAllPF').css('color','red');
          failure();
        } else {
          $('#getAllPF').html('Success!');
          success();
        }

        HTTPpost(originalNum);
      });
    }

    function HTTPpost(originalNum){
      var newHello = {
        'helloLanguage':'XX',
        'helloMessage':'Unit Test'};
      $.ajax({
        type: 'POST',
        dataType: 'json',
        data: newHello,
        url: uri + "Hello",
        success: function(data){
          $('#postPF').html('Success!');
          success();
          var newID = data.helloID;
          HTTPgetID(originalNum, newID);
        },
        error: function(){
          $('#postPF').html('Failure: No Results');
          $('#postPF').css('color','red');
          failure();
          HTTPgetID(originalNum, newID);
        }
      });
    }

    function HTTPgetID(originalNum, newID){
      var id = newID;
      $.getJSON(uri + 'Hello/' + id)
      .done(function (data) {
        $('<li>', { text: formatHelloItem(data) }).appendTo($('#getByIDResults'));
        
        if ($('#getByIDResults li').length == 0){
          $('#getByIDPF').html('Failure: No Results');
          $('#getByIDPF').css('color','red');
          failure();
        } else {
          $('#getByIDPF').html('Success!');
          success();
        }

        HTTPput(originalNum, newID);
      });
    }

    function HTTPput(originalNum, newID){
      var putHello = {
        'helloID':newID,
        'helloLanguage':'XX',
        'helloMessage':'Put Unit Test'};
      $.ajax({
        type: 'POST',
        dataType: 'json',
        data: putHello,
        url: uri + 'Hello?put=true',
        success: function(data){
          $('#putPF').html('Success!');
          success();
          HTTPgetLang(originalNum, newID, 'XX');
        },
        error: function(){
          $('#putPF').html('Failure: No Results');
          $('#putPF').css('color','red');
          failure();
          HTTPgetLang(originalNum, newID, 'XX');
        }
      });
    }

    function HTTPgetLang(originalNum, newID, lang){
      $.getJSON(uri + 'Hello?lang=XX')
      .done(function (data) {
        $('<li>', { text: formatHelloItem(data) }).appendTo($('#getByLangResults'));
        
        if ($('#getByLangResults li').length == 0){
          $('#getByLangPF').html('Failure: No Results');
          $('#getByLangPF').css('color','red');
          failure();
        } else {
          $('#getByLangPF').html('Success!');
          success();
        }

        HTTPdeleteTest(originalNum, newID);
      });
    }

    function HTTPdeleteTest(originalNum, newID){
      $.ajax({
        type: 'GET',
        url: uri + 'Hello/' + newID + '?delete=true',
        success: function(data){
          $('#deletePF').html('Success!');
          success();
          HTTPfinalHello(originalNum, newID);
        },
        error: function(){
          $('#deletePF').html('Failure: No Results');
          $('#deletePF').css('color','red');
          failure();
          HTTPfinalHello(originalNum, newID);
        }
      });
    }

    function HTTPfinalHello(originalNum, newID){
      $.getJSON(uri + "Hello")
      .done(function (data) {
        $.each(data, function (key, item) {
          $('<li>', { text: formatHelloItem(item) }).appendTo($('#finalGetResults'));
        });
        
        var finalNum = $('#finalGetResults li').length;

        if (finalNum == 0){
          $('#finalGetPF').html('Failure: No Results');
          $('#finalGetPF').css('color','red');
          failure();
        } else if (finalNum != originalNum) {
          $('#finalGetPF').html('Failure: Final and Original Num Differ');
          $('#finalGetPF').css('color','red');
          failure();
        } else {
          $('#finalGetPF').html('Success!');
          success();
        }

        AuthenticationAndValidation();
      });
    }

    function formatHelloItem(item) {
      return item.helloID + ': ' + item.helloLanguage + '- "' + item.helloMessage + '"';
    }

<!-- %%%%%%%%% Authentication And Validation %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% -->

    function AuthenticationAndValidation() {
      //Start
      //InvalidLogin();
      AUTHValidLogin();
      //InvalidToken();
      //ExpiredToken();
      //ValidToken();
    }

    function AUTHValidLogin(){
      var login = {
        'UserName':'UnitTester',
        'Password':'testpassword'};
      $.ajax({
        type: 'POST',
        dataType: 'json',
        data: login,
        url: uri + "Login",
        success: function(data){
          $('#VLoginPF').html('Success!');
          success();
          var newToken = data;
          AUTHValidToken(newToken);
        },
        error: function(){
          $('#VLoginPF').html('Failure: Not Allowed');
          $('#VLoginPF').css('color','red');
          failure();
          AUTHValidToken("");
        }
      });
    }

    function AUTHValidToken(newToken){
      $.getJSON(uri + "AuthTest",
        {token: newToken},
        function (data) {
          // On success, 'data' contains a list of products.
          $.each(data, function (key, item) {
            // Add a list item for the record.
            $('<li>', { text: formatAuthTestItem(item) }).appendTo($('#VTokenResults'));
          });
        }
      ).always(function(){
        var VResultCount = $('#VTokenResults li').length;

        if (VResultCount == 0){
          $('#VTokenPF').html('Failure: Not Accepted');
          $('#VTokenPF').css('color','red');
          failure();
        } else {
          $('#VTokenPF').html('Success!');
          success();
        }

        AUTHSecondLogin(VResultCount);
      });
    }

    function AUTHSecondLogin(VResultCount){
      var login = {
        'UserName':'testAdmin',
        'Password':'password'};
      $.ajax({
        type: 'POST',
        dataType: 'json',
        data: login,
        url: uri + "Login",
        success: function(data){
          var secondNewToken = data;
          //No success, this isn't a test.
          AUTHSecondValidToken(secondNewToken, VResultCount);
        },
        error: function(){
          $('#SecondVTokenPF').html('Failure: Second Login Not Allowed');
          $('#SecondVTokenPF').css('color','red');
          failure();
        }
      });
    }

    function AUTHSecondValidToken(secondNewToken, VResultCount){
      $.getJSON(uri + "AuthTest",
        {token: secondNewToken},
        function (data) {
          // On success, 'data' contains a list of products.
          $.each(data, function (key, item) {
            // Add a list item for the record.
            $('<li>', { text: formatAuthTestItem(item) }).appendTo($('#SecondVTokenResults'));
          });
        }
      ).always(function(){
        SecondVResultCount = $('#SecondVTokenResults li').length;

        if (SecondVResultCount == 0){
          $('#SecondVTokenPF').html('Failure: Not Accepted');
          $('#SecondVTokenPF').css('color','red');
          failure();
        } else if (SecondVResultCount == VResultCount) {
          $('#SecondVTokenPF').html('Possible Failure: Same Number of Results, verify with DB');
          $('#SecondVTokenPF').css('color','orange');
          failure();
        } else {
          $('#SecondVTokenPF').html('Success!');
          success();
        }

        AUTHInvalidLogin();
      });
    }

    function AUTHInvalidLogin(){
      var login = {
        'UserName':'UnitTester',
        'Password':'password'};
      $.ajax({
        type: 'POST',
        dataType: 'json',
        data: login,
        url: uri + "Login",
        success: function(data){
          $('#ILoginPF').html('Failure: Invalid Credentials Accepted');
          $('#ILoginPF').css('color','red');
          failure();
          AUTHInvalidToken();
        },
        error: function(){
          $('#ILoginPF').html('Success!');
          success();
          AUTHInvalidToken();
        }
      });
    }
    
    function AUTHInvalidToken(){
      $.getJSON(uri + "AuthTest",
        {token: '5d4acb08-ffc2-47e1-b3b9-e66e109542a9:0'},
        function (data) {
          // On success, 'data' contains a list of products.
          $.each(data, function (key, item) {
            // Add a list item for the record.
            $('<li>', { text: formatAuthTestItem(item) }).appendTo($('#ITokenResults'));
          });
        }
      ).always(function(){
        if ($('#ITokenResults li').length == 0){
          $('#ITokenPF').html('Success!');
          success();
        } else {
          $('#ITokenPF').html('Failure: Results Returned On Invalid Token');
          $('#ITokenPF').css('color','red');
          failure();
        }

        AUTHExpiredToken();
      });
    }

    function AUTHExpiredToken(){
      $.getJSON(uri + "AuthTest",
        {token: 'e6ee040e-dc94-421f-b6d9-614b80c7f709:4'},
        function (data) {
          // On success, 'data' contains a list of products.
          $.each(data, function (key, item) {
            // Add a list item for the record.
            $('<li>', { text: formatAuthTestItem(item) }).appendTo($('#ETokenResults'));
          });
        }
      ).always(function(){
        if ($('#ETokenResults li').length == 0){
          $('#ETokenPF').html('Success!');
          success();
        } else {
          $('#ETokenPF').html('Failure: Results Returned On Expired Token');
          $('#ETokenPF').css('color','red');
          failure();
        }

        PasswordChange();
      });
    }

    function formatAuthTestItem(item) {
      return item.AuthTestID + ': ' + item.UserName + '- "' + item.TestMessage + '"';
    }

<!-- %%%%%%%%% Password Change %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% -->

    function PasswordChange(){
      //Start
      PWCValidLogin();
      //InvalidOldPassword();
      //InvalidNewPassword();
      //ValidChange();
      //ValidReturn();
    }

    function PWCValidLogin(){
      var login = {
        'UserName':'UnitTester',
        'Password':'testpassword'};
      $.ajax({
        type: 'POST',
        dataType: 'json',
        data: login,
        url: uri + "Login",
        success: function(data){
          var token = data;
          PWCInvalidOldPassword(token);
        },
        error: function(){
          $('#IOldPF').html('Failure: Valid Login Not Accepted');
          $('#IOldPF').css('color','red');
          failure();
        }
      });
    }
    
    function PWCInvalidOldPassword(token){
      var model = {
        'OldPassword':'testpassword1',
        'NewPassword':'password',
        'ConfirmPassword':'password'};
      $.ajax({
        type: 'POST',
        dataType: 'json',
        data: model,
        url: uri + "Login?token=" + token,
        success: function(data){
          $('#IOldPF').html('Failure: Invalid Old Password Accepted');
          $('#IOldPF').css('color','red');
          failure();
        },
        error: function(jqXHR, exception){
          $('#IOldResult').html('' + jqXHR.status + ' - ' + jqXHR.responseText);
          $('#IOldPF').html('Success!');
          success();

          PWCInvalidShortPassword(token);
        }
      });
    }

    function PWCInvalidShortPassword(token){
      var model = {
        'OldPassword':'testpassword',
        'NewPassword':'1',
        'ConfirmPassword':'1'};
      $.ajax({
        type: 'POST',
        dataType: 'json',
        data: model,
        url: uri + "Login?token=" + token,
        success: function(data){
          $('#IShortPF').html('Failure: Invalid Short Password Accepted');
          $('#IShortPF').css('color','red');
          failure();
        },
        error: function(jqXHR, exception){
          $('#IShortResult').html('' + jqXHR.status + ' - ' + jqXHR.responseText);
          $('#IShortPF').html('Success!');
          success();

          PWCInvalidMismatchPassword(token);
        }
      });
    }

    function PWCInvalidMismatchPassword(token){
      var model = {
        'OldPassword':'testpassword',
        'NewPassword':'password',
        'ConfirmPassword':'password1'};
      $.ajax({
        type: 'POST',
        dataType: 'json',
        data: model,
        url: uri + "Login?token=" + token,
        success: function(data){
          $('#IMismatchPF').html('Failure: Invalid Mismatched Password Accepted');
          $('#IMismatchPF').css('color','red');
          failure();
        },
        error: function(jqXHR, exception){
          $('#IMismatchResult').html('' + jqXHR.status + ' - ' + jqXHR.responseText);
          $('#IMismatchPF').html('Success!');
          success();

          PWCValidChange(token);
        }
      });
    }

    function PWCValidChange(token){
      var model = {
        'OldPassword':'testpassword',
        'NewPassword':'password',
        'ConfirmPassword':'password'};
      $.ajax({
        type: 'POST',
        dataType: 'json',
        data: model,
        url: uri + "Login?token=" + token,
        success: function(data){
          $('#VChangePF').html('Success!');
          $('#VChangeResult').html(data);
          success();

          PWCValidReturn(token);
        },
        error: function(jqXHR, exception){
          $('#VChangeResult').html('' + jqXHR.status + ' - ' + jqXHR.responseText);
          $('#VChangePF').html('Failure: Valid Password Change Not Accepted');
          $('#VChangePF').css('color','red');
          failure();

          PWCValidReturn(token);
        }
      });
    }

    function PWCValidReturn(token){
      var model = {
        'OldPassword':'password',
        'NewPassword':'testpassword',
        'ConfirmPassword':'testpassword'};
      $.ajax({
        type: 'POST',
        dataType: 'json',
        data: model,
        url: uri + "Login?token=" + token,
        success: function(data){
          $('#VReturnPF').html('Success!');
          $('#VReturnResult').html(data);
          success();
          RoutineRetrieval();
        },
        error: function(jqXHR, exception){
          $('#VReturnResult').html('' + jqXHR.status + ' - ' + jqXHR.responseText);
          $('#VReturnPF').html('Failure: Valid Password Change Not Accepted');
          $('#VReturnPF').css('color','red');
          failure();
          RoutineRetrieval();
        }
      });
    }

<!-- %%%%%%%%% Routine Retrieval %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% -->

    function RoutineRetrieval(){
      RRlogin();
      //getAllRoutines()
      //getParentRoutines()
      //getJobCoachRoutines()
    }

    function RRlogin() {
      var login = {
        'UserName':'UnitTesterChild',
        'Password':'password'};
      $.ajax({
        type: 'POST',
        dataType: 'json',
        data: login,
        url: uri + "Login",
        success: function(data){
          var loginToken = data;
          //No success; not a test
          RRgetAllRoutines(loginToken);
        },
        error: function(){
          failure();
          alert('Login Failure');
        }
      });
    }
    
    function RRgetAllRoutines(loginToken) {
      $.getJSON(uri + "Routine",
        {token: loginToken},
        function (data) {
          // On success, 'data' contains a list of Routines.
          $.each(data, function (key, item) {
            // Add a list item for the record.
            $('<li>', { text: RRformatRoutine(item) }).appendTo($('#getAllRoutinesResults'));
          });
        }
      ).always(function(){
        if ($('#getAllRoutinesResults li').length == 3){
          $('#getAllRoutinesPF').html('Success!');
          success();
        } else {
          $('#getAllRoutinesPF').html('Failure: No Routines Returned');
          $('#getAllRoutinesPF').css('color','red');
          failure();
        }

        RRgetParentRoutines(loginToken);
      });
    }

    function RRgetParentRoutines(loginToken) {
      $.getJSON(uri + "Routine",
        {token: loginToken,assignedBy: "Parent"},
        function (data) {
          // On success, 'data' contains a list of Routines.
          $.each(data, function (key, item) {
            // Add a list item for the record.
            $('<li>', { text: RRformatRoutine(item) }).appendTo($('#getAllParentRoutinesResults'));
          });
        }
      ).always(function(){
        if ($('#getAllParentRoutinesResults li').length == 1){
          $('#getAllParentRoutinesPF').html('Success!');
          success();
        } else {
          $('#getAllParentRoutinesPF').html('Failure: No Routines Returned');
          $('#getAllParentRoutinesPF').css('color','red');
          failure();
        }

        RRgetJobCoachRoutines(loginToken);
      });
    }

    function RRgetJobCoachRoutines(loginToken) {
      $.getJSON(uri + "Routine",
        {token: loginToken,assignedBy: "Job Coach"},
        function (data) {
          // On success, 'data' contains a list of Routines.
          $.each(data, function (key, item) {
            // Add a list item for the record.
            $('<li>', { text: RRformatRoutine(item) }).appendTo($('#getAllCoachRoutinesResults'));
          });
        }
      ).always(function(){
        if ($('#getAllCoachRoutinesResults li').length == 2){
          $('#getAllCoachRoutinesPF').html('Success!');
          success();
        } else {
          $('#getAllCoachRoutinesPF').html('Failure: No Routines Returned');
          $('#getAllCoachRoutinesPF').css('color','red');
          failure();
        }

        complete();
      });
    }

    function RRformatRoutine(item) {
      return item.routineTitle + ': ' + item.userName + "'s Routine assigned by - " + item.creatorUserName;
    }
