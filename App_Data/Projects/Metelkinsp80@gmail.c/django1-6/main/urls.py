from django.urls import path
from . import views

urlpatterns = [
    path('', views.index),
    path('about', views.about),
    path('kafe', views.kafe),
    path('market', views.market),
    path('kontakt', views.kontakt),
    path('online', views.online)
]