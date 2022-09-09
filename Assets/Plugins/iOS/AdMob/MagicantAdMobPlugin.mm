#import <Foundation/Foundation.h>
#import <AdSupport/ASIdentifierManager.h>
#import <GoogleMobileAds/GoogleMobileAds.h>
#import <CommonCrypto/CommonDigest.h>

typedef void (*_magicant_admob_callbackInvoker)(void *csAdListener, int adCategory, int callbackType, int errorCode);

typedef NS_ENUM(NSInteger, Magicant_CallbackType) {
    AdLoaded,
    AdFailedToLoad,
    AdOpening,
    AdClosed,
    AdLeavingApplication,
    AdRewarded,
};

typedef NS_ENUM(NSInteger, Magicant_BannerAdPosition) {
    Top,
    Bottom,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
    Center,
};

typedef NS_ENUM(NSInteger, Magicant_BannerAdSize) {
    Size_320x50,
    Size_320x100,
    Size_300x250,
    Size_SmartBanner,
};

@interface MagicantAdMobBannerListener : NSObject <GADBannerViewDelegate>
- (instancetype)initWithAdListener:(void *)csAdListener adCategory:(int)adCategory callbackInvoker:(_magicant_admob_callbackInvoker)callbackInvoker;
@end

@implementation MagicantAdMobBannerListener {
    void *_csAdListener;
    int _adCategory;
    _magicant_admob_callbackInvoker _callbackInvoker;
}

- (instancetype)initWithAdListener:(void *)csAdListener
                      adCategory:(int)adCategory
                 callbackInvoker:(_magicant_admob_callbackInvoker)callbackInvoker {
    self = [super init];
    _csAdListener = csAdListener;
    _adCategory = adCategory;
    _callbackInvoker = callbackInvoker;
    return self;
}

- (void)adViewDidReceiveAd:(GADBannerView *)bannerView {
    NSLog(@"Magicant#AdMobPlugin : Banner Ad did receive. [%@, %@]", bannerView.adNetworkClassName, bannerView.adUnitID);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdLoaded, 0);
}

- (void)adView:(GADBannerView *)bannerView didFailToReceiveAdWithError:(GADRequestError *)error {
    NSLog(@"Magicant#AdMobPlugin : Banner Ad did fail to receive. [%@, %@]", bannerView.adNetworkClassName, bannerView.adUnitID);
    NSLog(@"[Error] %@", error);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdFailedToLoad, (int)error.code);
}

- (void)adViewWillPresentScreen:(GADBannerView *)bannerView {
    NSLog(@"Magicant#AdMobPlugin : Banner Ad will present screen. [%@, %@]", bannerView.adNetworkClassName, bannerView.adUnitID);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdOpening, 0);
}

- (void)adViewWillDismissScreen:(GADBannerView *)bannerView {
    NSLog(@"Magicant#AdMobPlugin : Banner Ad will dismiss screen. [%@, %@]", bannerView.adNetworkClassName, bannerView.adUnitID);
}

- (void)adViewDidDismissScreen:(GADBannerView *)bannerView {
    NSLog(@"Magicant#AdMobPlugin : Banner Ad did dismiss screen. [%@, %@]", bannerView.adNetworkClassName, bannerView.adUnitID);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdClosed, 0);
}

- (void)adViewWillLeaveApplication:(GADBannerView *)bannerView {
    NSLog(@"Magicant#AdMobPlugin : Banner Ad will leave application. [%@, %@]", bannerView.adNetworkClassName, bannerView.adUnitID);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdLeavingApplication, 0);
}
@end


@interface MagicantAdMobInterstitialListener : NSObject <GADInterstitialDelegate>
- (instancetype)initWithAdListener:(void *)csAdListener adCategory:(int)adCategory callbackInvoker:(_magicant_admob_callbackInvoker)callbackInvoker;
@end

@implementation MagicantAdMobInterstitialListener {
    void *_csAdListener;
    int _adCategory;
    _magicant_admob_callbackInvoker _callbackInvoker;
}

- (instancetype)initWithAdListener:(void *)csAdListener
                        adCategory:(int)adCategory
                   callbackInvoker:(_magicant_admob_callbackInvoker)callbackInvoker {
    self = [super init];
    _csAdListener = csAdListener;
    _adCategory = adCategory;
    _callbackInvoker = callbackInvoker;
    return self;
}

- (void)interstitialDidReceiveAd:(GADInterstitial *)ad {
    NSLog(@"Magicant#AdMobPlugin : Interstitial Ad did receive. [%@, %@]", ad.adNetworkClassName, ad.adUnitID);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdLoaded, 0);
}

- (void)interstitial:(GADInterstitial *)ad didFailToReceiveAdWithError:(GADRequestError *)error {
    NSLog(@"Magicant#AdMobPlugin : Interstitial Ad did fail to receive. [%@, %@]", ad.adNetworkClassName, ad.adUnitID);
    NSLog(@"[Error] %@", error);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdFailedToLoad, (int)error.code);
}

- (void)interstitialWillPresentScreen:(GADInterstitial *)ad {
    NSLog(@"Magicant#AdMobPlugin : Interstitial Ad will present screen. [%@, %@]", ad.adNetworkClassName, ad.adUnitID);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdOpening, 0);
}

- (void)interstitialDidFailToPresentScreen:(GADInterstitial *)ad {
    NSLog(@"Magicant#AdMobPlugin : Interstitial Ad did fail to present screen. [%@, %@]", ad.adNetworkClassName, ad.adUnitID);
}

- (void)interstitialWillDismissScreen:(GADInterstitial *)ad {
    NSLog(@"Magicant#AdMobPlugin : Interstitial Ad will dismiss screen. [%@, %@]", ad.adNetworkClassName, ad.adUnitID);
}

- (void)interstitialDidDismissScreen:(GADInterstitial *)ad {
    NSLog(@"Magicant#AdMobPlugin : Interstitial Ad did dismiss screen. [%@, %@]", ad.adNetworkClassName, ad.adUnitID);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdClosed, 0);
}

- (void)interstitialWillLeaveApplication:(GADInterstitial *)ad {
    NSLog(@"Magicant#AdMobPlugin : Interstitial Ad will leave application. [%@, %@]", ad.adNetworkClassName, ad.adUnitID);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdLeavingApplication, 0);
}
@end


@interface MagicantAdMobVideoListener : NSObject <GADRewardBasedVideoAdDelegate>
- (instancetype)initWithAdListener:(void *)csAdListener adCategory:(int)adCategory callbackInvoker:(_magicant_admob_callbackInvoker)callbackInvoker;
@end

@implementation MagicantAdMobVideoListener {
    void *_csAdListener;
    int _adCategory;
    _magicant_admob_callbackInvoker _callbackInvoker;
}

- (instancetype)initWithAdListener:(void *)csAdListener
                        adCategory:(int)adCategory
                   callbackInvoker:(_magicant_admob_callbackInvoker)callbackInvoker {
    self = [super init];
    _csAdListener = csAdListener;
    _adCategory = adCategory;
    _callbackInvoker = callbackInvoker;
    return self;
}

- (void)rewardBasedVideoAd:(GADRewardBasedVideoAd *)rewardBasedVideoAd
   didRewardUserWithReward:(GADAdReward *)reward {
    NSLog(@"Magicant#AdMobPlugin : Rewared Based Video Ad rewarded. [%@]", rewardBasedVideoAd.adNetworkClassName);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdRewarded, 0);
}

- (void)rewardBasedVideoAd:(GADRewardBasedVideoAd *)rewardBasedVideoAd
    didFailToLoadWithError:(NSError *)error {
    NSLog(@"Magicant#AdMobPlugin : Rewared Based Video Ad did fail to load. [%@]", rewardBasedVideoAd.adNetworkClassName);
    NSLog(@"[Error] %@", error);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdFailedToLoad, (int)error.code);
}

- (void)rewardBasedVideoAdDidReceiveAd:(GADRewardBasedVideoAd *)rewardBasedVideoAd {
    NSLog(@"Magicant#AdMobPlugin : Rewared Based Video Ad did receive. [%@]", rewardBasedVideoAd.adNetworkClassName);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdLoaded, 0);
}

- (void)rewardBasedVideoAdDidOpen:(GADRewardBasedVideoAd *)rewardBasedVideoAd {
    NSLog(@"Magicant#AdMobPlugin : Rewared Based Video Ad did open. [%@]", rewardBasedVideoAd.adNetworkClassName);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdOpening, 0);
}

- (void)rewardBasedVideoAdDidStartPlaying:(GADRewardBasedVideoAd *)rewardBasedVideoAd {
    NSLog(@"Magicant#AdMobPlugin : Rewared Based Video Ad did start playing. [%@]", rewardBasedVideoAd.adNetworkClassName);
}

- (void)rewardBasedVideoAdDidCompletePlaying:(GADRewardBasedVideoAd *)rewardBasedVideoAd {
    NSLog(@"Magicant#AdMobPlugin : Rewared Based Video Ad did complete playing. [%@]", rewardBasedVideoAd.adNetworkClassName);
}

- (void)rewardBasedVideoAdDidClose:(GADRewardBasedVideoAd *)rewardBasedVideoAd {
    NSLog(@"Magicant#AdMobPlugin : Rewared Based Video Ad did close. [%@]", rewardBasedVideoAd.adNetworkClassName);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdClosed, 0);
}

- (void)rewardBasedVideoAdWillLeaveApplication:(GADRewardBasedVideoAd *)rewardBasedVideoAd {
    NSLog(@"Magicant#AdMobPlugin : Rewared Based Video Ad will leave application. [%@]", rewardBasedVideoAd.adNetworkClassName);
    _callbackInvoker(_csAdListener, _adCategory, Magicant_CallbackType::AdLeavingApplication, 0);
}
@end


#ifdef __cplusplus
extern "C" {
#endif

static NSString *_magicant_admob_toMd5Str(const char *baseStr) {
    unsigned char buff[CC_MD5_DIGEST_LENGTH];
    CC_MD5(baseStr, (CC_LONG)strlen(baseStr), buff);
    NSMutableString *md5Str = [NSMutableString stringWithCapacity:CC_MD5_DIGEST_LENGTH * 2];
    for(int i = 0; i < CC_MD5_DIGEST_LENGTH; i++) {
        [md5Str appendFormat:@"%02x",buff[i]];
    }
    return md5Str;
}

static NSString *_magicant_admob_getDeviceId() {
    const char *devideId = [ASIdentifierManager sharedManager].advertisingIdentifier.UUIDString.UTF8String;
    NSString *md5Str = _magicant_admob_toMd5Str(devideId);
    return md5Str;
}

static CGRect _magicant_admob_toBannerAdFrame(CGSize adSize, int adPositionNum) {
    CGSize parentSize = UnityGetGLViewController().view.frame.size;
    Magicant_BannerAdPosition adPosition = (Magicant_BannerAdPosition)adPositionNum;
    
    switch (adPosition) {
        case Magicant_BannerAdPosition::Top:
            return CGRectMake((parentSize.width - adSize.width) / 2,
                              0,
                              adSize.width,
                              adSize.height);
        case Magicant_BannerAdPosition::Bottom:
            return CGRectMake((parentSize.width - adSize.width) / 2,
                              parentSize.height - adSize.height,
                              adSize.width,
                              adSize.height);
        case Magicant_BannerAdPosition::TopLeft:
            return CGRectMake((parentSize.width - adSize.width) / 2,
                              parentSize.height - adSize.height,
                              adSize.width,
                              adSize.height);
        case Magicant_BannerAdPosition::TopRight:
            return CGRectMake((parentSize.width - adSize.width),
                              0,
                              adSize.width,
                              adSize.height);
        case Magicant_BannerAdPosition::BottomLeft:
            return CGRectMake(0,
                              (parentSize.height - adSize.height) / 2,
                              adSize.width,
                              adSize.height);
        case Magicant_BannerAdPosition::BottomRight:
            return CGRectMake((parentSize.width - adSize.width),
                              (parentSize.height - adSize.height) / 2,
                              adSize.width,
                              adSize.height);
        case Magicant_BannerAdPosition::Center:
            return CGRectMake((parentSize.width - adSize.width) / 2,
                              (parentSize.height - adSize.height) / 2,
                              adSize.width,
                              adSize.height);
        default:
            return CGRectMake((parentSize.width - adSize.width) / 2,
                              0,
                              adSize.width,
                              adSize.height);
    }
}

    
void _magicant_admob_release(void *instance) {
    CFRelease((CFTypeRef)instance);
}

GADRequest *_magicant_admob_create_request() {
    GADRequest *request = [GADRequest request];
    CFRetain((CFTypeRef)request);
    return request;
}

void _magicant_admob_application_initialize(const char *appId) {
    [GADMobileAds configureWithApplicationID:[NSString stringWithUTF8String:appId]];
}

GADBannerView *_magicant_admob_banner_initialize(int adSizeNum, const char *adUnitId) {
    UIView *parent = UnityGetGLViewController().view;
    Magicant_BannerAdSize adSize = (Magicant_BannerAdSize)adSizeNum;
    GADAdSize gadAdSize;
    switch (adSize) {
        case Magicant_BannerAdSize::Size_320x50:
            gadAdSize = kGADAdSizeBanner;
            break;
        case Magicant_BannerAdSize::Size_320x100:
            gadAdSize = kGADAdSizeLargeBanner;
            break;
        case Magicant_BannerAdSize::Size_300x250:
            gadAdSize = kGADAdSizeMediumRectangle;
            break;
        case Magicant_BannerAdSize::Size_SmartBanner:
            if (parent.frame.size.width > parent.frame.size.height) {
                gadAdSize = kGADAdSizeSmartBannerPortrait;
            } else {
                gadAdSize = kGADAdSizeSmartBannerLandscape;
            }
            break;
    }
    GADBannerView *bannerView = [[GADBannerView alloc] initWithAdSize:gadAdSize];
    bannerView.adUnitID = [NSString stringWithUTF8String:adUnitId];
    bannerView.rootViewController = UnityGetGLViewController();
    CFRetain((CFTypeRef)bannerView);
    return bannerView;
}

MagicantAdMobBannerListener *_magicant_admob_banner_setListener(GADBannerView *bannerView, void *csAdListener, int adCategoryNum,
                                                                _magicant_admob_callbackInvoker callbackInvoker) {
    MagicantAdMobBannerListener *listener =
    [[MagicantAdMobBannerListener alloc] initWithAdListener:csAdListener adCategory:adCategoryNum
                                            callbackInvoker:callbackInvoker];
    bannerView.delegate = listener;
    CFRetain((CFTypeRef)listener);
    return listener;
}

void _magicant_admob_banner_setAdPositionWithAlign(GADBannerView *bannerView, int adPositionNum) {
    bannerView.frame = _magicant_admob_toBannerAdFrame(bannerView.frame.size, adPositionNum);
}

void _magicant_admob_banner_setAdPositionWithXY(GADBannerView *bannerView, int x, int y) {
    bannerView.frame = CGRectMake(x, y, bannerView.frame.size.width, bannerView.frame.size.height);
}

void _magicant_admob_banner_loadAd(GADRequest *request, GADBannerView *bannerView, bool isTest) {
    NSLog(@"Magicant#AdMobPlugin : Banner Ad load. [%@]", bannerView.adUnitID);
    if (isTest) {
        NSString *deviceId = _magicant_admob_getDeviceId();
        [request setTestDevices:@[deviceId]];
    }
    [bannerView loadRequest:request];
}

void _magicant_admob_banner_show(GADBannerView *bannerView) {
    NSLog(@"Magicant#AdMobPlugin : Banner Ad show. [%@, %@]", bannerView.adNetworkClassName, bannerView.adUnitID);
    [UnityGetGLViewController().view addSubview:bannerView];
}

void _magicant_admob_banner_hide(GADBannerView *bannerView) {
    NSLog(@"Magicant#AdMobPlugin : Banner Ad hide. [%@, %@]", bannerView.adNetworkClassName, bannerView.adUnitID);
    if (bannerView.superview) {
        [bannerView removeFromSuperview];
    }
}

    
GADInterstitial *_magicant_admob_interstitial_initialize(const char *adUnitId) {
    GADInterstitial *interstitial = [[GADInterstitial alloc] initWithAdUnitID:[NSString stringWithUTF8String:adUnitId]];
    CFRetain((CFTypeRef)interstitial);
    return interstitial;
}

MagicantAdMobInterstitialListener *_magicant_admob_interstitial_setListener(GADInterstitial *interstitial, void *csAdListener, int adCategoryNum,
                                                                            _magicant_admob_callbackInvoker callbackInvoker) {
    MagicantAdMobInterstitialListener *listener =
    [[MagicantAdMobInterstitialListener alloc] initWithAdListener:csAdListener adCategory:adCategoryNum
                                                  callbackInvoker:callbackInvoker];
    interstitial.delegate = listener;
    CFRetain((CFTypeRef)listener);
    return listener;
}

void _magicant_admob_interstitial_loadAd(GADRequest *request, GADInterstitial *interstitial, bool isTest) {
    NSLog(@"Magicant#AdMobPlugin : Interstitial Ad load. [%@]", interstitial.adUnitID);
    if (isTest) {
        NSString *deviceId = _magicant_admob_getDeviceId();
        [request setTestDevices:@[deviceId]];
    }
    [interstitial loadRequest:request];
}

bool _magicant_admob_interstitial_isLoaded(GADInterstitial *interstitial) {
    if (UnityGetGLViewController().presentedViewController) return false;
    return [interstitial isReady];
}

void _magicant_admob_interstitial_show(GADInterstitial *interstitial) {
    NSLog(@"Magicant#AdMobPlugin : Interstitial Ad show. [%@, %@]", interstitial.adNetworkClassName, interstitial.adUnitID);
    [interstitial presentFromRootViewController:UnityGetGLViewController()];
}


MagicantAdMobVideoListener *_magicant_admob_video_setListener(void *csAdListener, int adCategoryNum,
                                                                     _magicant_admob_callbackInvoker callbackInvoker) {
    MagicantAdMobVideoListener *listener =
    [[MagicantAdMobVideoListener alloc] initWithAdListener:csAdListener adCategory:adCategoryNum
                                           callbackInvoker:callbackInvoker];
    [GADRewardBasedVideoAd sharedInstance].delegate = listener;
    CFRetain((CFTypeRef)listener);
    return listener;
}

void _magicant_admob_video_loadAd(GADRequest *request, const char *adUnitId, bool isTest) {
    NSLog(@"Magicant#AdMobPlugin : Rewarded Video Ad load. [%s]", adUnitId);
    if (isTest) {
        NSString *deviceId = _magicant_admob_getDeviceId();
        [request setTestDevices:@[deviceId]];
    }
    [[GADRewardBasedVideoAd sharedInstance] loadRequest:request withAdUnitID:[NSString stringWithUTF8String:adUnitId]];
}

bool _magicant_admob_video_isLoaded() {
    if (UnityGetGLViewController().presentedViewController) return false;
    return [[GADRewardBasedVideoAd sharedInstance] isReady];
}

void _magicant_admob_video_show() {
    GADRewardBasedVideoAd *rewardedVideoAd = [GADRewardBasedVideoAd sharedInstance];
    NSLog(@"Magicant#AdMobPlugin : Rewarded Video Ad show. [%@]", rewardedVideoAd.adNetworkClassName);
    [rewardedVideoAd presentFromRootViewController:UnityGetGLViewController()];
}

#ifdef __cplusplus
}
#endif
