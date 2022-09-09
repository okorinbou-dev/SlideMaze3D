#import <FirebaseCore/FirebaseCore.h>

#ifdef __cplusplus
extern "C" {
#endif

void _firebase_initialize() {
    [FIRApp configure];
}

NSMutableDictionary *_firebase_new_dictionary() {
    NSMutableDictionary *dictionary = [NSMutableDictionary dictionary];
    CFRetain((CFTypeRef)dictionary);
    return dictionary;
}

void _firebase_delete_dictionary(NSMutableDictionary *dictionary) {
    CFRelease((CFTypeRef)dictionary);
}

void _firebase_add_dictionary_long(NSMutableDictionary *dictionary, const char *key, long value) {
    NSString *keyStr = [NSString stringWithUTF8String:key];
    [dictionary setObject:@(value) forKey:keyStr];
}

void _firebase_add_dictionary_double(NSMutableDictionary *dictionary, const char *key, double value) {
    NSString *keyStr = [NSString stringWithUTF8String:key];
    [dictionary setObject:@(value) forKey:keyStr];
}

void _firebase_add_dictionary_bool(NSMutableDictionary *dictionary, const char *key, bool value) {
    NSString *keyStr = [NSString stringWithUTF8String:key];
    [dictionary setObject:@(value) forKey:keyStr];
}

void _firebase_add_dictionary_string(NSMutableDictionary *dictionary, const char *key, const char *value) {
    NSString *keyStr = [NSString stringWithUTF8String:key];
    NSString *valueStr = [NSString stringWithUTF8String:value];
    [dictionary setObject:valueStr forKey:keyStr];
}

#ifdef __cplusplus
}
#endif
