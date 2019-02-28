"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var router_1 = require("@angular/router");
var register_component_1 = require("./register/register.component");
var login_component_1 = require("./login/login.component");
exports.ROUTES = router_1.RouterModule.forChild([
    { path: 'register', component: register_component_1.RegisterComponent },
    { path: 'login', component: login_component_1.LoginComponent }
]);
//# sourceMappingURL=auth-routing.module.js.map