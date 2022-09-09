#import <FirebaseAnalytics/FirebaseAnalytics.h>

#ifdef __cplusplus
extern "C" {
#endif

void _firebase_analytics_logEvent(const char *name, NSMutableDictionary *parameter) {
    NSString *eventNameStr = [NSString stringWithUTF8String:name];
    [FIRAnalytics logEventWithName:eventNameStr parameters:parameter];
}

NSMutableDictionary *_firebase_analytics_new_parameter() {
    NSMutableDictionary *param = [NSMutableDictionary dictionary];
    CFRetain((CFTypeRef)param);
    return param;
}

void _firebase_analytics_delete_parameter(NSMutableDictionary *parameter) {
    CFRelease((CFTypeRef)parameter);
}
    
void _firebase_analytics_add_parameter_long(NSMutableDictionary *parameter, const char *key, long value) {
    NSString *keyStr = [NSString stringWithUTF8String:key];
    [parameter setObject:@(value) forKey:keyStr];
}

void _firebase_analytics_add_parameter_double(NSMutableDictionary *parameter, const char *key, double value) {
    NSString *keyStr = [NSString stringWithUTF8String:key];
    [parameter setObject:@(value) forKey:keyStr];
}

void _firebase_analytics_add_parameter_string(NSMutableDictionary *parameter, const char *key, const char *value) {
    NSString *keyStr = [NSString stringWithUTF8String:key];
    NSString *valueStr = [NSString stringWithUTF8String:value];
    [parameter setObject:valueStr forKey:keyStr];
}
    
void _firebase_analytics_set_user_property(const char *name, const char *property) {
    NSString *nameStr = [NSString stringWithUTF8String:name];
    NSString *propertyStr = [NSString stringWithUTF8String:property];
    [FIRAnalytics setUserPropertyString:propertyStr forName:nameStr];
}

void _firebase_analytics_set_user_id(const char *userId) {
    NSString *userIdStr = [NSString stringWithUTF8String:userId];
    [FIRAnalytics setUserID:userIdStr];
}

#ifdef __cplusplus
}
#endif
