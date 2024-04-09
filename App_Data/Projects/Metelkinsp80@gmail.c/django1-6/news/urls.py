from django.urls import path
from . import views

urlpatterns = [
    path('', views.news_home, name='news_home'),
    path('about', views.about),
    path('kafe', views.kafe),
    path('market', views.market),
    path('kontakt', views.kontakt),
    path('index', views.index),
    path('login', views.MyprojectLoginView.as_view(), name='login'),
    path('register', views.RegisterUserView.as_view(), name='register')
]