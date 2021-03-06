# pi  

docker

``` 
https://download.docker.com/linux/raspbian/dists/stretch/pool/stable/armhf/
1. sudo apt-get install -y libltdl7
2. wget https://download.docker.com/linux/raspbian/dists/stretch/pool/stable/armhf/docker-ce_18.09.0~3-0~raspbian-stretch_armhf.deb
3. wget https://download.docker.com/linux/raspbian/dists/stretch/pool/stable/armhf/docker-ce-cli_18.09.0~3-0~raspbian-stretch_armhf.deb
4. wget https://download.docker.com/linux/raspbian/dists/stretch/pool/stable/armhf/containerd.io_1.2.0~rc.2-1_armhf.deb
5. sudo dpkg -i containerd.io_1.2.0~rc.2-1_armhf.deb
6. sudo dpkg -i docker-ce-cli_18.09.0~3-0~raspbian-stretch_armhf.deb
7. sudo dpkg -i docker-ce_18.09.0~3-0~raspbian-stretch_armhf.deb
8. sudo usermod -aG docker ${USER}


``` 
```
    curl -fsSL https://get.docker.com | bash -s docker --mirror Aliyun
    sudo usermod -aG docker $USER
```
install k8s
https://blog.csdn.net/liukuan73/article/details/83150473

``` 
with root:
curl https://mirrors.aliyun.com/kubernetes/apt/doc/apt-key.gpg | apt-key add -
echo "deb https://mirrors.aliyun.com/kubernetes/apt/ kubernetes-xenial main" > /etc/apt/sources.list.d/kubernetes.list
apt-get install -y kubeadm

or
sudo apt-get update && sudo apt-get install -y apt-transport-https curl 
sudo curl -s https://mirrors.aliyun.com/kubernetes/apt/doc/apt-key.gpg | sudo apt-key add - 
sudo tee /etc/apt/sources.list.d/kubernetes.list <<-'EOF' 
deb https://mirrors.aliyun.com/kubernetes/apt kubernetes-xenial main 
EOF 
sudo apt-get update

``` 

禁用SELINUX

``` 
树莓派系统默认不安装selinux，无需设置。假如安装了的话，按如下步骤禁用selinux
临时禁用（重启后失效）
$ sudo setenforce 0                 #0代表permissive 1代表enforcing
永久禁用
$ sudo vi /etc/selinux/config
SELINUX=permissive                            
备注：

        kubelet目前对selinux的支持还不好，需要禁用掉。
        不禁用selinux的话有时还会出现明明容器里是root身份运行，操作挂载进来的主机文件夹时没有权限的情况，这时候要么配置selinux的权限，要么就禁掉selinux
        另外，当docker的storage-driver使用overlay2的时候，低版本内核的selinux不支持overlay2文件驱动，docker启动时设置为--selinux-enabled会失败报错:“Error starting daemon: SELinux is not supported with the overlay2 graph driver on this kernel”，需设置--selinux-enabled=false
``` 

准备docker images

```   
mirrorgooglecontainers:
docker pull mirrorgooglecontainers/kube-apiserver-arm:v1.13.1
docker pull mirrorgooglecontainers/kube-controller-manager-arm:v1.13.1
docker pull mirrorgooglecontainers/kube-scheduler-arm:v1.13.1
docker pull mirrorgooglecontainers/kube-proxy-arm:v1.13.1 
docker pull mirrorgooglecontainers/pause-arm:3.1
docker pull mirrorgooglecontainers/etcd-arm:3.2.24

docker pull argon99/coredns-rpi



docker tag mirrorgooglecontainers/kube-apiserver-arm:v1.13.1 k8s.gcr.io/kube-apiserver:v1.13.1
docker tag mirrorgooglecontainers/kube-controller-manager-arm:v1.13.1 k8s.gcr.io/kube-controller-manager:v1.13.1
docker tag mirrorgooglecontainers/kube-scheduler-arm:v1.13.1 k8s.gcr.io/kube-scheduler:v1.13.1
docker tag mirrorgooglecontainers/kube-proxy-arm:v1.13.1 k8s.gcr.io/kube-proxy:v1.13.1
docker tag mirrorgooglecontainers/pause-arm:3.1 k8s.gcr.io/pause:3.1
docker tag mirrorgooglecontainers/etcd-arm:3.2.24 k8s.gcr.io/etcd:3.2.24
docker tag argon99/coredns-rpi k8s.gcr.io/coredns:1.2.6





```   


开启memory的cgroup功能

```   
树莓派是基于debian的系统，而debian默认没有mount memory的cgroup，这里需要手动开启
编辑/boot/cmdline.txt，在该行的末尾添加此文本，但不要创建任何新行：
cgroup_enable=memory
重启
```  

禁用swap

``` 
1. sudo swapoff -a
2. 同时还需要修改/etc/fstab文件，注释掉 SWAP 的自动挂载，防止机子重启后swap启用。
备注：
        Kubernetes 1.8开始要求关闭系统的Swap，如果不关闭，默认配置下kubelet将无法启动，虽然可以通过kubelet的启动参数--fail-swap-on=false更改这个限制，但不建议，最好还是不要开启swap。
        一些为什么要关闭swap的讨论：
        <1>https://github.com/kubernetes/kubernetes/issues/7294
        <2>https://github.com/kubernetes/kubernetes/issues/53533
``` 

配置使用阿里云镜像仓库加速

``` 
https://cr.console.aliyun.com/cn-hangzhou/mirrors


sudo tee /etc/docker/daemon.json <<-'EOF' 
{ "registry-mirrors": ["https://4erji9ui.mirror.aliyuncs.com"], "iptables": false, "ip-masq": false, "storage-driver": "overlay2", "graph": "/home/pi/docker" 
}
EOF



``` 
Your Kubernetes master has initialized successfully!

To start using your cluster, you need to run the following as a regular user:

  mkdir -p $HOME/.kube
  sudo cp -i /etc/kubernetes/admin.conf $HOME/.kube/config
  sudo chown $(id -u):$(id -g) $HOME/.kube/config

You should now deploy a pod network to the cluster.
Run "kubectl apply -f [podnetwork].yaml" with one of the options listed at:
  https://kubernetes.io/docs/concepts/cluster-administration/addons/

You can now join any number of machines by running the following on each node
as root:

  kubeadm join 192.168.31.133:6443 --token 1e242f.z7iu3hnvbq3xlkyi --discovery-token-ca-cert-hash sha256:a143d509219aa09f286c51e07cde7d2c9c1b9266a6c7d85500ca1bffb77216c3


route -n
sudo route del default gw 10.242.146.1
