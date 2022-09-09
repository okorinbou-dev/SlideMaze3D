#import <UIKit/UIKit.h>

#ifdef __cplusplus
extern "C" {
#endif

static UIView *_coverView = nil;

void _LoadingViewController_show() {
    if (_coverView) {
        return;
    }
    UIView *unityView = UnityGetGLViewController().view;
    _coverView = [[UIView alloc] initWithFrame:unityView.frame];
    [_coverView setBackgroundColor:[UIColor colorWithRed:0 green:0 blue:0 alpha:0.8]];
    [_coverView setUserInteractionEnabled:YES];
    [unityView setUserInteractionEnabled:NO];
    
    UILabel *loadingLabel = [[UILabel alloc] initWithFrame:unityView.frame];
    [loadingLabel setTextAlignment:NSTextAlignmentCenter];
    [loadingLabel setFont:[UIFont systemFontOfSize:25.0f]];
    [loadingLabel setText:@"Loading .."];
    [loadingLabel setTextColor:[UIColor whiteColor]];
    [_coverView addSubview:loadingLabel];
    
    [unityView addSubview:_coverView];
}
    
void _LoadingViewController_hide() {
    if (_coverView) {
        [_coverView removeFromSuperview];
        _coverView = nil;
        [UnityGetGLViewController().view setUserInteractionEnabled:YES];
    }
}

void _LoadingViewController_bringToFront() {
    if (_coverView) {
        [_coverView.superview bringSubviewToFront:_coverView];
    }
}
    
bool _LoadingViewController_isShowing() {
    return (_coverView != nil);
}

#ifdef __cplusplus
}
#endif
